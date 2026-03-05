using Assets.Player;
using Assets.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

public class ColonialShop : MonoBehaviour
{
    public event UnityAction OnVehicleTypeChange;

    [SerializeField]
    private UIDocument _document;
    private VisualElement _shop;
    private VisualElement _currentTankPanel;
    private ListView _vehiclesView;
    private VehiclePresenters _allVehiclePresenters;
    private VehiclePresenter _currentTankPresenter;
    private Button _changeTankButton;

    [Inject]
    private IPlayerSettings _playerSettings;

    void Start()
    {
        _shop = _document.rootVisualElement.Q<VisualElement>("ColonialShopWindow");
        _vehiclesView = _shop.Q<VisualElement>("ShopPanel").Q<ListView>("AvailableVehiclesView");
        _currentTankPanel = _shop.Q<VisualElement>("ShopPanel").Q<VisualElement>("CurrentTankPanel");
        _changeTankButton = _shop.Q<Button>("ChangeTankButton");
        _allVehiclePresenters = Resources.Load<VehiclePresenters>("VehiclePresenters");
        _vehiclesView.itemsSource = _allVehiclePresenters.Data;

        _vehiclesView.selectionChanged += VehicleClicked;


        RefreshCurrentVehicle();
        _changeTankButton.clicked += ChangeVehicle;
    }

    private void VehicleClicked(IEnumerable<object> enumerable)
    {
        _currentTankPanel.dataSource = enumerable.First();
    }

    private void ChangeVehicle()
    {

        _playerSettings.CurrentVehicle = ((VehiclePresenter)_vehiclesView.selectedItem).VehicleType;
        _playerSettings.SaveSettings();
        OnVehicleTypeChange?.Invoke();
    }

    private void RefreshCurrentVehicle()
    {
        _vehiclesView.selectedIndex = _allVehiclePresenters.Data.IndexOf(_allVehiclePresenters.Data.First(x => x.VehicleType == _playerSettings.CurrentVehicle));
    }

    private void OnDestroy()
    {
        _vehiclesView.selectionChanged -= VehicleClicked;
        _changeTankButton.clicked -= ChangeVehicle;
    }

}
