using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class BattleRoutine : MonoBehaviour
{
    [SerializeField]
    private Shuttle _shuttle;

    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _shuttlePoint;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private UIDocument _document;

    private VisualElement _completeContractWindow;
    private Button _close_completeContractWindowButton;
    private Label _targetsLeftLabel;

    [Inject]
    ISceneAssetFactory _sceneAssetFactory;

    [Inject]
    private IVehicleFactory _vehicleFactory;

    [Inject]
    private IPlayerSettings _playerSettings;

    [Inject]
    private IContractsManager _contractsManager;

    private LifeManager _lifeManager;
    private Vehicle _playerVehicle;
    void Start()
    {
        _completeContractWindow = _document.rootVisualElement.Q<VisualElement>("ContractCompleteWindow");
        _close_completeContractWindowButton = _completeContractWindow.Q<Button>("CloseButton");
        _close_completeContractWindowButton.clicked += CloseCompleteContractWindow;
        _targetsLeftLabel = _document.rootVisualElement.Q<Label>("TargetsLeftLabel");
        // var flea = _sceneAssetFactory.CreateAsset<BoomFlea>();
        // flea.transform.position = new Vector3(-4, 0, 0);
        _shuttle.transform.position = _shuttlePoint.position;
        _shuttle.MoveToPoint(_startPoint.position, LandPlayerAndTakeOff);
        _lifeManager = GetComponent<LifeManager>();
        if (_contractsManager.CurrentContract.Type == ContractType.Cleanse)
            _lifeManager.AllEnemyDead += CompleteContract;
        _lifeManager.EnemyLiveCount += TargetsCountChanged;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    private void TargetsCountChanged(int targetsLeft)
    {
        _targetsLeftLabel.text = $"Осталось целей: {targetsLeft}";
    }

    private void CloseCompleteContractWindow()
    {
        _completeContractWindow.style.display = DisplayStyle.None;
    }

    private void LandPlayerAndTakeOff()
    {
        _playerVehicle = _vehicleFactory.CreateVehicle(_playerSettings.CurrentVehicle);
        _playerVehicle.transform.position = _startPoint.position;
        _playerVehicle.GetComponent<TankController>().CallToEvacuate += OnEvacuate;
        _playerVehicle.GetComponent<TankController>().Die += OnEvacuate;
        _cameraController.BindObject(_playerVehicle.gameObject);
        _shuttle.TakeOff(() =>  _shuttle.transform.position = _shuttlePoint.position );
    }

    private void OnEvacuate(BaseEntity player)
    {
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

    private void CompleteContract()
    {
        _contractsManager.CurrentContractStatus = ContractStatus.Completed;
        _contractsManager.SaveData();
        _completeContractWindow.style.display = DisplayStyle.Flex;
    }

    private void CheckIfContractFailedOnExit()
    {
        if (_contractsManager.CurrentContractStatus != ContractStatus.Completed)
            _contractsManager.CurrentContractStatus = ContractStatus.Failed;
        _contractsManager.SaveData();
    }


    private void OnDestroy()
    {
        CheckIfContractFailedOnExit();
        if (_playerVehicle != null)
        {
            _playerVehicle.GetComponent<TankController>().CallToEvacuate -= OnEvacuate;
            _playerVehicle.GetComponent<TankController>().Die -= OnEvacuate;
        }
        _lifeManager.AllEnemyDead -= CompleteContract;
        _close_completeContractWindowButton.clicked -= CloseCompleteContractWindow;
        _lifeManager.EnemyLiveCount -= TargetsCountChanged;        
    }
}
