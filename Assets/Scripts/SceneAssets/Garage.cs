using Assets.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

public class Garage : MonoBehaviour
{
    public event UnityAction OnVehicleTypeChange;

    [SerializeField]
    private UIDocument _document;
    private VisualElement _garage;
    private VisualElement _currentTankPanel;
    private ListView _vehiclesView;
    private VehiclePresenters _allVehiclePresenters;
    private VehiclePresenter _currentTankPresenter;
    private Button _changeTankButton;

    [Inject]
    private IPlayerSettings _playerSettings;

    void Start()
    {
        _garage = _document.rootVisualElement.Q<VisualElement>("GarageWindow");
        _vehiclesView = _garage.Q<VisualElement>("GaragePanel").Q<ListView>("AvailableVehiclesView");
        _currentTankPanel = _garage.Q<VisualElement>("GaragePanel").Q<VisualElement>("CurrentTankPanel");
        _changeTankButton = _garage.Q<Button>("ChangeTankButton");
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
