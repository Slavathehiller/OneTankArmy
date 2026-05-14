using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.ObjectPool;
using Assets.Scripts.SceneNavigation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public abstract class BattleRoutine : MonoBehaviour
{
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
    private RenderTexture _minimapRenderTexture;

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

    protected LifeManager _lifeManager;
    protected Vehicle _playerVehicle;
    protected TankController _playerController;
    
    protected abstract int[] GetEnemiesCount();

    protected abstract void ContractConditionsInit();

    void Start()
    {
        _completeContractWindow = _document.rootVisualElement.Q<VisualElement>("ContractCompleteWindow");

        var minimapImage = _document.rootVisualElement.Q<Image>("MinimapImage");
        if (minimapImage != null)
            minimapImage.image = _minimapRenderTexture;


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
            //if (!string.IsNullOrEmpty(_sceneNavigator.StartPointName)) 
            //{
            //    var startPoint = GameObject.Find(_sceneNavigator.StartPointName);
            //    if (startPoint == null)
            //    {
            //        Debug.LogError($"Не найден объект с именем {_sceneNavigator.StartPointName}");
            //    }
            //    else
            //        _startPoint.transform.position = startPoint.transform.position;
            //}
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
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    public void SpawnEnemies()
    {
        var enemiesCount = GetEnemiesCount();
        for (var i = 0; i < GetEnemiesCount().Length; i++)
        {
            AIEnemy enemy;
            switch (i)
            {
                case 0:
                    {
                        for (var j = 0; j < enemiesCount[i]; j++)
                        {
                            enemy = _sceneAssetFactory.CreateAsset<AcidCockroach>();
                            PlaceEnemy(enemy);
                        }
                        break;
                    }
                case 1:
                    {
                        for (var j = 0; j < enemiesCount[i]; j++)
                        {
                            enemy = _sceneAssetFactory.CreateAsset<BoomFlea>();
                            PlaceEnemy(enemy);
                        }
                        break;
                    }
                case 2:
                    {
                        for (var j = 0; j < enemiesCount[i]; j++)
                        {
                            enemy = _sceneAssetFactory.CreateAsset<GiantScolopendra>();
                            PlaceEnemy(enemy);
                        }
                        break;
                    }
                case 3:
                    {
                        for (var j = 0; j < enemiesCount[i]; j++)
                        {
                            enemy = _sceneAssetFactory.CreateAsset<FireMantiss>();
                            PlaceEnemy(enemy);
                        }
                        break;
                    }
            }
        }
    }

    private void PlaceEnemy(AIEnemy enemy)
    {
        var spawnPointIndex = Random.Range(0, _spawnPoints.Count);
        var spawnPoint = _spawnPoints[spawnPointIndex];
        enemy.transform.position = spawnPoint.position;
        _spawnPoints.Remove(spawnPoint);
        var angle = Random.Range(0, 360);
        enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
        _playerController = _playerVehicle.GetComponent<TankController>();
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
        _playerVehicle.GetComponent<TankController>().CallToEvacuate -= OnEvacuate;
        _playerVehicle.GetComponent<TankController>().Die -= OnEvacuate;
        _playerSettings.CurrentHealth = _playerVehicle.Health;
        _playerSettings.SaveSettings();
        _contractsManager.SaveData();
        Destroy(_playerVehicle.gameObject);
        _sceneNavigator.ResetData();
        _shuttle.TakeOff(() => SceneManager.LoadScene(Scenes.OUTPOST_SCENE));
    }

    protected void CompleteContract()
    {
        _contractsManager.CurrentContractStatus = ContractStatus.Completed;
        _contractsManager.SaveData();
        _completeContractWindow.style.display = DisplayStyle.Flex;
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
