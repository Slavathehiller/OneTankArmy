using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.SceneNavigation;
using System.Collections.Generic;
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
    private List<Transform> _spawnPoints;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    protected UIDocument _document;

    private VisualElement _completeContractWindow;

    [Inject]
    ISceneAssetFactory _sceneAssetFactory;

    [Inject]
    private IVehicleFactory _vehicleFactory;

    [Inject]
    protected IPlayerSettings _playerSettings;

    [Inject]
    protected IContractsManager _contractsManager;

    [Inject]
    private ISceneNavigator _sceneNavigator;

    protected LifeManager _lifeManager;
    protected Vehicle _playerVehicle;
    protected abstract int[] GetEnemiesCount();

    protected abstract void ContractConditionsInit();

    void Start()
    {
        _completeContractWindow = _document.rootVisualElement.Q<VisualElement>("ContractCompleteWindow");

        if (_sceneNavigator.GoingFromAnotherScene)
        {
            if (!string.IsNullOrEmpty(_sceneNavigator.StartPointName)) 
            {
                var startPoint = GameObject.Find(_sceneNavigator.StartPointName);
                if (startPoint == null)
                {
                    Debug.LogError($"Не найден объект с именем {_sceneNavigator.StartPointName}");
                }
                else
                    _startPoint.transform.position = startPoint.transform.position;
            }
            CreatePlayerVehicle();
        }
        else
        {
            _shuttle.transform.position = _shuttlePoint.position;
            _shuttle.MoveToPoint(_startPoint.position, LandPlayerAndTakeOff);
        }
        SpawnEnemies();
        _lifeManager = GetComponent<LifeManager>();
        ContractConditionsInit();
        _lifeManager.Init();
        LateStart();
    }

    protected virtual void LateStart() { }
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
        _playerVehicle.transform.position = _startPoint.position;
        _cameraController.BindObject(_playerVehicle.gameObject);
        _playerVehicle.GetComponent<TankController>().CallToEvacuate += OnEvacuate;
        _playerVehicle.GetComponent<TankController>().Die += OnDie;
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
            _playerVehicle.GetComponent<TankController>().CallToEvacuate -= OnEvacuate;
            _playerVehicle.GetComponent<TankController>().Die -= OnEvacuate;
        }
    }
}
