using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

public class BattleRoutine : MonoBehaviour
{
    [Inject]
    ISceneAssetFactory _sceneAssetFactory;

    [Inject]
    private IVehicleFactory _vehicleFactory;

    [Inject]
    private IPlayerSettings _playerSettings;

    [SerializeField]
    private Shuttle _shuttle;

    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _shuttlePoint;

    [SerializeField]
    private CameraController _cameraController;

    private Vehicle _playerVehicle;
    void Start()
    {
        // var flea = _sceneAssetFactory.CreateAsset<BoomFlea>();
        // flea.transform.position = new Vector3(-4, 0, 0);
        _shuttle.transform.position = _shuttlePoint.position;
        _shuttle.MoveToPoint(_startPoint.position, LandPlayerAndTakeOff);
    }
  
    private void LandPlayerAndTakeOff()
    {
        _playerVehicle = _vehicleFactory.CreateVehicle(_playerSettings.CurrentVehicle);
        _playerVehicle.transform.position = _startPoint.position;
        _cameraController.BindObject(_playerVehicle.gameObject);
        _shuttle.TakeOff(() =>  _shuttle.transform.position = _shuttlePoint.position );
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
            Application.Quit();
    }

}
