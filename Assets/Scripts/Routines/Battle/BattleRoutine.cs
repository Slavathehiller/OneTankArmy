using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.ObjectPool;
using Assets.Scripts.SceneNavigation;
using NavMeshPlus.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public abstract class BattleRoutine : MonoBehaviour
{

    private enum MinimapMode
    {
        None,
        Normal,
        Zoom
    }

    [SerializeField]
    private Shuttle _shuttle;

    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _shuttlePoint;

    [SerializeField]
    protected List<Portal> _portals;

    [SerializeField]
    private List<Transform> _spawnPoints;

    [SerializeField]
    private List<Transform> _portalPoints;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    protected UIDocument _document;

    [SerializeField]
    private Camera _minimapCamera;

    [SerializeField] 
    private RenderTexture _minimapRenderTexture;
    [SerializeField]
    private RenderTexture _minimapZoomRenderTexture;

    private Image _minimapImage;
    private Image _minimapImage_zoom;

    private Image _minimapImageContainer;
    private Image _minimapImageZoomContainer;

    private MinimapMode _minimapMode;

    private VisualElement _completeContractWindow;

    [Inject]
    protected ISceneAssetFactory _sceneAssetFactory;

    [Inject]
    private IVehicleFactory _vehicleFactory;

    [Inject]
    protected IPlayerSettings _playerSettings;

    [Inject]
    protected IContractsManager _contractsManager;

    [Inject]
    protected ISceneNavigator _sceneNavigator;

    [Inject]
    private IMissilePool _missilePool;

    [Inject]
    private IPlanetManager _planetManager;

    protected LifeManager _lifeManager;
    protected Vehicle _playerVehicle;
    protected TankController _playerController;
    
    protected abstract int[] GetEnemiesCount();

    protected abstract void ContractConditionsInit();

    void Start()
    {
        _minimapMode = MinimapMode.Normal;
        _completeContractWindow = _document.rootVisualElement.Q<VisualElement>("ContractCompleteWindow");

        _minimapImage = _document.rootVisualElement.Q<Image>("MinimapImage");
        if (_minimapImage != null)
            _minimapImage.image = _minimapRenderTexture;

        _minimapImage_zoom = _document.rootVisualElement.Q<Image>("MinimapImage_zoom");
        if (_minimapImage_zoom != null)
            _minimapImage_zoom.image = _minimapZoomRenderTexture;

        _minimapImageContainer = _document.rootVisualElement.Q<Image>("MinimapImageContainer");
        _minimapImageZoomContainer = _document.rootVisualElement.Q<Image>("MinimapImageZoomContainer");

        if (_sceneNavigator.NavigationVector == NavigationVector.GoingToMission)
        {
            PositionPortals();
            _shuttle.transform.position = _shuttlePoint.position;
            _shuttle.MoveToPoint(_startPoint.position, LandPlayerAndTakeOff);
        }
        else
        {
            foreach(var portal in _portals)
            {
                if (_sceneNavigator.PortalsCoords.TryGetValue(portal.Name, out var coords))
                {
                    portal.transform.position = coords;
                }
            }

            _missilePool.Clear();
            CreatePlayerVehicle();
        }
        SpawnEnemies();
        _lifeManager = GetComponent<LifeManager>();
        ContractConditionsInit();
        _lifeManager.Init();
        LateStart();
    }

    private void PositionPortals()
    {
        if (_portals.Count > 0 && _portalPoints.Count > 0)
        {
            foreach (var portal in _portals)
            {
                var portalPointIndex = Random.Range(0, _portalPoints.Count);
                portal.transform.position = _portalPoints[portalPointIndex].position;
                _portalPoints.RemoveAt(portalPointIndex);
                if (_portalPoints.Count < 1)
                    break;
            }
            foreach (var portal in _portals)
            {
                if (!_sceneNavigator.PortalsCoords.ContainsKey(portal.Name))
                    _sceneNavigator.PortalsCoords.Add(portal.Name, portal.transform.position);
            }
        }
    }
    protected virtual void LateStart() {}

    private void SetMiniMap()
    {
        _minimapImageContainer.style.display = DisplayStyle.None;
        _minimapImageZoomContainer.style.display = DisplayStyle.None;

        if (_minimapMode == MinimapMode.Normal)
        {
            _minimapImageContainer.style.display = DisplayStyle.Flex;
            if (_minimapCamera != null)
                _minimapCamera.targetTexture = _minimapRenderTexture;
        }
        if (_minimapMode == MinimapMode.Zoom)
        {
            _minimapImageZoomContainer.style.display = DisplayStyle.Flex;
            if (_minimapCamera != null)
                _minimapCamera.targetTexture = _minimapZoomRenderTexture;
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.M))
        {
            _minimapMode++;
            if ((int)_minimapMode > 2)
                _minimapMode = 0;

            SetMiniMap();
        }
        UpdateActions();
    }

    protected virtual void UpdateActions() {}

    public virtual void SpawnEnemies()
    {
        var enemiesCount = GetEnemiesCount();
        for (var i = 0; i < GetEnemiesCount().Length; i++)
        {
            for (var j = 0; j < enemiesCount[i]; j++)
            {
                switch (i)
                {
                    case 0: SpawnEnemy<AcidCockroach>();
                        break;
                    case 1: SpawnEnemy<BoomFlea>();
                        break;
                    case 2: SpawnEnemy<GiantScolopendra>();
                        break;
                    case 3: SpawnEnemy<FireMantiss>();
                        break;
                }
            }
        }
    }

    protected T SpawnEnemy<T>() where T : AIEnemy
    {
        var enemy = _sceneAssetFactory.CreateAsset<T>();
        PlaceEnemy(enemy);
        return enemy;
    }

    private void PlaceEnemy(AIEnemy enemy)
    {
        var spawnPointIndex = Random.Range(0, _spawnPoints.Count);
        var spawnPoint = _spawnPoints[spawnPointIndex];
        enemy.transform.position = spawnPoint.position;
        _spawnPoints.Remove(spawnPoint);
        var angle = Random.Range(0, 360);
        enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        InitEnemy(enemy);
    }

    protected void InitEnemy(AIEnemy enemy)
    {
        enemy.SetAgentOn();
        var minimapMarkPrefab = Resources.Load<GameObject>("Prefabs/red-dot-mark");
        var minimapMark = GameObject.Instantiate(minimapMarkPrefab);
        enemy.GetMinimapMark(minimapMark);
    }

    private void CreatePlayerVehicle()
    {
        _playerVehicle = _vehicleFactory.CreateVehicle(_playerSettings.CurrentVehicle);
        PlayerVehicleInit();
    }
    protected virtual void PlayerVehicleInit() 
    {
        _playerVehicle.Health = _playerSettings.CurrentHealth;
        if (_sceneNavigator.NavigationVector == NavigationVector.GoingToMission)
            _playerVehicle.transform.position = _startPoint.position;
        else
        {
            var portalToGO = _portals.First(x => x.Name == _sceneNavigator.PortalToGo);
            _playerVehicle.transform.position = portalToGO.ExitPoint.position;
            _playerVehicle.transform.rotation = portalToGO.ExitPoint.rotation;
        }
        _cameraController.BindObject(_playerVehicle.gameObject);
        _playerController = _playerVehicle.TankController;
        _playerController.CallToEvacuate += OnEvacuate;
        _playerController.Die += OnDie;
        _playerController.GoingToPortal += PlayerGoToPortal;
    }

    private void PlayerGoToPortal(Portal portal)
    {
        portal.LoadNextScene();
    }

    private void LandPlayerAndTakeOff()
    {
        CreatePlayerVehicle();
        _shuttle.TakeOff(() =>  _shuttle.transform.position = _shuttlePoint.position );
    }


    protected virtual void OnDie(BaseEntity player)
    {
        _playerVehicle.ControlOff();
        CheckIfContractFailedOnExit();
        Invoke("Evacuate", 2);
    }
    protected virtual void OnEvacuate(BaseEntity player)
    {
        _playerVehicle.EvacuateFlareOn();
        _playerVehicle.ControlOff();
        CheckIfContractFailedOnExit();
        Invoke("Evacuate", 2);
    }
    private void Evacuate()
    {
        _shuttle.gameObject.SetActive(true);
        _shuttle.MoveToPoint(_playerVehicle.transform.position, PickupPlayerAndTakeOff);
    }

    private void PickupPlayerAndTakeOff()
    {
        _playerController.CallToEvacuate -= OnEvacuate;
        _playerController.Die -= OnEvacuate;
        _playerSettings.CurrentHealth = _playerVehicle.Health;
        _playerSettings.SaveSettings();
        _contractsManager.SaveData();
        Destroy(_playerVehicle.gameObject);
        _sceneNavigator.ResetData();
        _shuttle.TakeOff(() => SceneManager.LoadScene(_planetManager.CurrentPlanet.OutpostScene));
    }


    protected void SetCompleteContractMessage(string message)
    {
        var contractCompleteLabel = _document.rootVisualElement.Q<Label>("ContractCompleteLabel");
        contractCompleteLabel.text = message;
    }

    protected void CompleteContract()
    {
        _contractsManager.CurrentContractStatus = ContractStatus.Completed;
        _contractsManager.SaveData();
        _completeContractWindow.style.display = DisplayStyle.Flex;
    }

    protected void FailContract()
    {
        _contractsManager.CurrentContractStatus = ContractStatus.Failed;
        _contractsManager.SaveData();
        _completeContractWindow.style.display = DisplayStyle.Flex;
        SetCompleteContractMessage("Контракт провален. Нажмите Ctrl-E для эвакуации");
    }

    protected void CheckIfContractFailedOnExit()
    {
        if (_contractsManager.CurrentContractStatus != ContractStatus.Completed && _contractsManager.CurrentContract.QuestItemNeed == 0)
            _contractsManager.CurrentContractStatus = ContractStatus.Failed;
        _contractsManager.SaveData();
    }


    private void OnDestroy()
    {
        OnDestroyAction();
    }

    protected virtual void OnDestroyAction()
    {
        CheckIfContractFailedOnExit();
        if (_playerVehicle != null)
        {
            _playerController.CallToEvacuate -= OnEvacuate;
            _playerController.Die -= OnEvacuate;
            _playerController.GoingToPortal -= PlayerGoToPortal;
        }
        _missilePool.Clear();
    }
}
