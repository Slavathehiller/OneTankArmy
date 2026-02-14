using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class ColonialShop : MonoBehaviour
{
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
        _changeTankButton = _shop.Q<VisualElement>("ShopPanel").Q<Button>("ChangeTankButton");
        _allVehiclePresenters = Resources.Load<VehiclePresenters>("VehiclePresenters");
        _vehiclesView.itemsSource = _allVehiclePresenters.Data;
        
        RefreshCurrentVehicle();

        _changeTankButton.clicked += ChangeVehicle;
    }

    private void ChangeVehicle()
    {
        _playerSettings.CurrentVehicle = ((VehiclePresenter)_vehiclesView.selectedItem).VehicleType;
        _playerSettings.SaveSettings();
        RefreshCurrentVehicle();
    }

    private void RefreshCurrentVehicle()
    {
        _currentTankPanel.dataSource = _allVehiclePresenters.Data.First(x => x.VehicleType == _playerSettings.CurrentVehicle);
    }

}
