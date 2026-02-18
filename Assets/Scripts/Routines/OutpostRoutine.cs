using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.Player;
using Assets.Scripts.SceneAssets;
using Assets.Vehicles;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class OutpostRoutine : MonoBehaviour
{
    [Serializable]
    private class TimeResponse
    {
        public string datetime; 
    }

    [SerializeField]
    private Shuttle _shuttle;
    [SerializeField]
    private Transform _repairPoint;
    [SerializeField]
    private Transform _defaultPointPoint;

    [SerializeField]
    private ColonialShop _shop;

    [SerializeField]
    private UIWindowActivator _colonialShopActivator;

    [SerializeField]
    private UIDocument _document;
    private Button _takeOffButtonButton;
    private VisualElement _rapairingWindow;

    private Vehicle _playerAvatar;

    [Inject]
    private IPlayerSettings _playerSettings;

    [Inject]
    private IVehicleFactory _vehicleFactory;
    void Start()
    {
        _takeOffButtonButton = _document.rootVisualElement.Q<Button>("TakeOffButton");
        _takeOffButtonButton.clicked += TakeOff;
        _rapairingWindow = _document.rootVisualElement.Q<VisualElement>("RepairingWndow");
        _rapairingWindow.Q<Button>("CloseButton").clicked += () => _rapairingWindow.style.visibility = Visibility.Hidden;
        _shop.OnVehicleTypeChange += ChangeVehicle;

        _colonialShopActivator.WindowAvailable = ColonialShopAvailable;

        CreateAvatar(true);
    }

    private void Update()
    {
        if (DateTime.Now >= _playerSettings.RepairEndTime)
        {
            _playerSettings.RepairEndTime = null;
            _playerSettings.CurrentHealth = _playerAvatar.MaxHealth;
            _playerSettings.SaveSettings();
            _playerAvatar.Health = _playerAvatar.MaxHealth;
            _playerAvatar.transform.position = _defaultPointPoint.position;
        }
    }

    private bool ColonialShopAvailable()
    {
        if (_playerSettings.RepairEndTime != null)
        {
            _rapairingWindow.style.visibility = Visibility.Visible;
            return false;
        }
        return true;
    }

    private void ChangeVehicle()
    {
        Destroy(_playerAvatar.gameObject);
        CreateAvatar(false);
    }

    public void CreateAvatar(bool initial)
    {
        _playerAvatar = _vehicleFactory.CreateVehicle(_playerSettings.CurrentVehicle);
        _playerAvatar.Health = _playerSettings.CurrentHealth;
        _playerAvatar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _playerAvatar.ControlOff();
        _playerAvatar.transform.position = _defaultPointPoint.position;

        if (!initial)
        {
            _playerSettings.CurrentHealth = _playerAvatar.MaxHealth;
            return;
        }

        if (_playerSettings.RepairEndTime != null)
        {
            if (_playerSettings.RepairEndTime < GetDateTime())
            {
                _playerSettings.RepairEndTime = null;
                _playerSettings.CurrentHealth = _playerAvatar.MaxHealth;
                _playerSettings.SaveSettings();
                _playerAvatar.Health = _playerAvatar.MaxHealth;
            }
            else
                _playerAvatar.transform.position = _repairPoint.position;
            return;
        }

        if (_playerAvatar.Health < _playerAvatar.MaxHealth)
                StartRepair();        
    }

    private void TakeOff()
    {
        if (_playerSettings.CurrentHealth < _playerAvatar.MaxHealth)
        {
            _rapairingWindow.style.visibility = Visibility.Visible;
            return;
        }
        _takeOffButtonButton.style.visibility = Visibility.Hidden;
        _shuttle.GetComponent<Collider2D>().enabled = false;
        _shuttle.TakeOff(()=> SceneManager.LoadScene(Scenes.BATTLE_SCENE));
    }
    
    private void StartRepair()
    {
        _playerAvatar.transform.position = _repairPoint.position;
        _playerSettings.RepairEndTime = GetDateTime().AddMinutes((_playerAvatar.MaxHealth - _playerSettings.CurrentHealth) / 2);
    }

    private DateTime GetDateTime()
    {
        //using (UnityWebRequest www = UnityWebRequest.Get("https://worldtimeapi.org/api/ip"))
        //{
        //    if (www.result == UnityWebRequest.Result.Success)
        //    {
        //        string json = www.downloadHandler.text;
        //        var dateTimeStr = JsonUtility.FromJson<TimeResponse>(json).datetime;
        //        return DateTime.Parse(dateTimeStr).ToUniversalTime();

        //    }
        //    else
        //    {
        //        Debug.LogError("Ошибка времени: " + www.error);
                return DateTime.Now;
        //    }
        //}
    }

    private void OnDestroy()
    {
        _takeOffButtonButton.clicked -= TakeOff;
        _shop.OnVehicleTypeChange -= ChangeVehicle;
    }
}
