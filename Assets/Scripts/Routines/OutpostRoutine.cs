using Assets.Player;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.NPC;
using Assets.Scripts.Player;
using Assets.Scripts.SceneAssets;
using Assets.Vehicles;
using System;
using Unity.VisualScripting;
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
    private ControlCenter _controlCenter;

    [SerializeField]
    private ErrorController _errorController;

    [SerializeField]
    private UIWindowActivator _colonialShopActivator;

    [SerializeField]
    private UIWindowActivator _controlCenterActivator;

    [SerializeField]
    private UIDocument _document;
    private Button _takeOffButtonButton;
    private VisualElement _rapairingWindow;
    private Button _currentContractButton;

    private Vehicle _playerAvatar;
    private SimpleMover _playerAvatarMover;

    private VisualElement _currentContractWindow;
    private Button _currentContractTakeOffButton;
    private VisualElement _checkCurrentContractStatusWindow;
    private Button _closeCheckCurrentContractStatusWindowButton;

    [Inject]
    private IPlayerSettings _playerSettings;

    [Inject]
    private IContractsManager _contractManager;

    [Inject]
    private IVehicleFactory _vehicleFactory;
    void Start()
    {
        _takeOffButtonButton = _document.rootVisualElement.Q<Button>("TakeOffButton");
        _takeOffButtonButton.clicked += TakeOff;

        _rapairingWindow = _document.rootVisualElement.Q<VisualElement>("RepairingWndow");
        _rapairingWindow.Q<Button>("CloseButton").clicked += () => _rapairingWindow.style.visibility = Visibility.Hidden;

        _currentContractWindow = _document.rootVisualElement.Q<VisualElement>("CurrentContractWindow");
        _currentContractWindow.Q<Button>("CloseButton").clicked += () => _currentContractWindow.style.display = DisplayStyle.None;

        _checkCurrentContractStatusWindow = _document.rootVisualElement.Q<VisualElement>("CheckCurrentContractStatusWindow");
        _closeCheckCurrentContractStatusWindowButton = _checkCurrentContractStatusWindow.Q<Button>("CloseButton");
        _closeCheckCurrentContractStatusWindowButton.clicked += CloseCheckCurrentContractStatusWindow;
        _currentContractTakeOffButton = _currentContractWindow.Q<Button>("TakeOffButton");
        _currentContractTakeOffButton.clicked += () => _currentContractWindow.style.display = DisplayStyle.None;
        _currentContractTakeOffButton.clicked += TakeOff;

        _shop.OnVehicleTypeChange += ChangeVehicle;
        _controlCenter.OnContractSigned += ContractSignet;


        _colonialShopActivator.WindowAvailable = ColonialShopAvailable;

        _controlCenterActivator.WindowAvailable = ControlCenterAvailable;

        _currentContractButton = _document.rootVisualElement.Q<VisualElement>("MainUI").Q<Button>("CurrentContractButton");
        _currentContractButton.clicked += ShowCurrentContactWindow;
        CheckCurrentContractStatus();

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

        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            SceneManager.LoadScene(Scenes.MAIN_MENU);
        }
    }

    private void ContractSignet()
    {
        _document.rootVisualElement.Q<VisualElement>("OperationCenterWndow").style.visibility = Visibility.Hidden;
        CheckCurrentContractButton();
        ShowCurrentContactWindow();
    }

    private void CheckCurrentContractStatus()
    {
        CheckCurrentContractButton();
        if (_contractManager.CurrentContractStatus == ContractStatus.Unsigned || _contractManager.CurrentContractStatus == ContractStatus.Signed)
            return;

        var statusLabel = _checkCurrentContractStatusWindow.Q<Label>("MessageLabel");
        if (_contractManager.CurrentContractStatus == ContractStatus.Completed)
        {
            statusLabel.text = "Контракт выполнен";
        }
        if (_contractManager.CurrentContractStatus == ContractStatus.Failed)

        {
            statusLabel.text = "Контракт провален";
        }

        _checkCurrentContractStatusWindow.style.display = DisplayStyle.Flex;
        _contractManager.CurrentContractStatus = ContractStatus.Unsigned;
        _contractManager.CurrentContract = null;
        _contractManager.SaveData();
        CheckCurrentContractButton();
    }

    private void CheckCurrentContractButton()
    {
        if (_contractManager.CurrentContractStatus == ContractStatus.Unsigned)
            _currentContractButton.style.display = DisplayStyle.None;
        else
            _currentContractButton.style.display = DisplayStyle.Flex;
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

    private bool ControlCenterAvailable()
    {
        if (_contractManager.CurrentContract != null)
        {
            _errorController.ShowError("Имеется незакрытый контракт.");            
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
        _playerAvatar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        _playerAvatar.ControlOff();
        _playerAvatar.transform.position = _defaultPointPoint.position;
        _playerAvatarMover = _playerAvatar.AddComponent<SimpleMover>();

        if (!initial)                                                   //Не первое создание (поменяли танк)
        {
            _playerSettings.CurrentHealth = _playerAvatar.MaxHealth;
            return;
        }

        if (_playerSettings.RepairEndTime != null && _playerSettings.RepairEndTime <= GetDateTime()) //Если ремонт закончился оффлайн
        {
            _playerSettings.RepairEndTime = null;
            _playerSettings.CurrentHealth = _playerAvatar.MaxHealth;
            _playerSettings.SaveSettings();
            _playerAvatar.Health = _playerAvatar.MaxHealth;
            return;
        }

        if (_playerSettings.RepairEndTime == null && _playerSettings.CurrentHealth < _playerAvatar.MaxHealth) //Если ремонт не начат, но здоровье меньше максимального (прилетели побитые)
                StartRepair();

        if (_playerSettings.RepairEndTime != null && _playerSettings.RepairEndTime > GetDateTime()) //Если ремонт еще не закончился (зашли до окончания ремонта)
            _playerAvatar.transform.position = _repairPoint.position;
    }

    private void ShowCurrentContactWindow()
    {
        _currentContractWindow.style.display = DisplayStyle.Flex;
        _currentContractWindow.dataSource = _contractManager.CurrentContract;
    }
    private void TakeOff()
    {
        if (_playerSettings.CurrentHealth < _playerAvatar.MaxHealth)
        {
            _rapairingWindow.style.visibility = Visibility.Visible;
            return;
        }

        if (_contractManager.CurrentContract == null)
        {
            _errorController.ShowError("Не заключен контракт. Вылет невозможен.");
            return;
        }


        _takeOffButtonButton.style.visibility = Visibility.Hidden;
        _shuttle.GetComponent<Collider2D>().enabled = false;
        _playerAvatarMover.MoveTo(_shuttle.transform.position, 2, () => 
                  { 
                    Destroy(_playerAvatar.gameObject);
                    _shuttle.TakeOff(() => SceneManager.LoadScene(Scenes.BATTLE_SCENE));
                  });        
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

    private void CloseCheckCurrentContractStatusWindow() 
    {
        _checkCurrentContractStatusWindow.style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        _takeOffButtonButton.clicked -= TakeOff;
        _currentContractTakeOffButton.clicked -= TakeOff;
        _shop.OnVehicleTypeChange -= ChangeVehicle;
        _controlCenter.OnContractSigned -= ContractSignet;
        _closeCheckCurrentContractStatusWindowButton.clicked -= CloseCheckCurrentContractStatusWindow;
        _playerSettings.SaveSettings();
    }
}
