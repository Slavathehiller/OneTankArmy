using Assets.Player;
using Assets.Scripts.Orbit;
using Assets.Scripts.SceneNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class OrbitRoutine : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [SerializeField]
    private List<PlanetPresenter> _planetPresenters;

    [SerializeField] 
    private PanelSettings _panelSettings;

    [SerializeField]
    private SpaceShip _spaceship;

    [SerializeField] 
    private VisualTreeAsset _planetNameTemplate;

    private VisualElement _upperInfoPanel;
    private Button _takeoffButton;
    private Button _landingButton;
    private Image _selectFrame;

    private List<PlanetInfo> _planetsInfo;
    private PlanetInfo _selectedPlanetInfo;
    private bool _shipInRoute;

    private PlanetPresenter SelectedPlanetPresenter
    {
        get
        {
            if (_selectedPlanetInfo == null)
                return null;
            return _planetPresenters.First(x => x.ID == _selectedPlanetInfo.ID);
        }
    }

    [Inject]
    private IPlayerSettings _playerSettings;

    [Inject]
    private ISceneNavigator _sceneNavigator;

    void Start()
    {
        _planetsInfo = Resources.Load<Planets>("Planets").Data;
        CreatePlanetNames();

        _upperInfoPanel = _document.rootVisualElement.Q<VisualElement>("UpperInfoPanel");
        _landingButton = _document.rootVisualElement.Q<Button>("LandingButton");
        _takeoffButton = _document.rootVisualElement.Q<Button>("TakeoffButton");
        _selectFrame = _document.rootVisualElement.Q<Image>("SelectFrame");       

        foreach (PlanetPresenter planetPresenter in _planetPresenters)
        {
            planetPresenter.OnSelect += PlanetSelect;
        }

        _takeoffButton.clicked += MoveToPlanet;
        _landingButton.clicked += PlanetLanding;

        _spaceship.HasArrived += ShipArrived;

        PlanetSelect(_playerSettings.CurrentPlanetID);
        _spaceship.transform.position = SelectedPlanetPresenter.ShipPoint.position;
        RefreshNavigationButtons();
    }

    private Vector2 GetPanelScaling()
    {
        var scale = new Vector2(1, 1);
        if (_panelSettings != null && _panelSettings.scaleMode == PanelScaleMode.ScaleWithScreenSize)
        {

            float scaleX = (float)_panelSettings.referenceResolution.x / Screen.width;
            float scaleY = (float)_panelSettings.referenceResolution.y / Screen.height;

            scale.x *= scaleX;
            scale.y *= scaleY;
        }
        return scale;
    }

    private void CreatePlanetNames()
    {
        foreach (var presenter in _planetPresenters)
        {
            if (presenter.ID < 0)
                continue;
            var info = _planetsInfo.FirstOrDefault(p => p.ID == presenter.ID);
            if (info == null) continue;

            var worldPos = presenter.NamePoint.position;

            if (presenter.TryGetComponent(out SpriteRenderer sr))
                worldPos.x -= sr.bounds.extents.x * presenter.transform.localScale.x;

            var screenPos = Camera.main.WorldToScreenPoint(worldPos);

            var uiPosition = new Vector2(screenPos.x, Screen.height - screenPos.y);


            var labelInstance = _planetNameTemplate.Instantiate();
            labelInstance.style.position = Position.Absolute;

            var scale = GetPanelScaling();

            uiPosition.x *= scale.x;
            uiPosition.y *= scale.y;

            labelInstance.style.left = uiPosition.x;
            labelInstance.style.top = uiPosition.y;


            var nameLabel = labelInstance.Q<Label>("Name");
            if (nameLabel != null)
                nameLabel.text = info.Name;

            _document.rootVisualElement.Add(labelInstance);
        }
    }

    private void ShipArrived()
    {
        _shipInRoute = false;
        RefreshNavigationButtons();
    }

    private void RefreshNavigationButtons()
    {
        if (_selectedPlanetInfo.ID < 0)
        {
            _takeoffButton.style.display = DisplayStyle.None;
            _landingButton.style.display = DisplayStyle.None;
            return;
        }

        if (_playerSettings.CurrentPlanetID == _selectedPlanetInfo.ID)
        {
            _takeoffButton.style.display = DisplayStyle.None;
            _landingButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            _takeoffButton.style.display = DisplayStyle.Flex;
            _landingButton.style.display = DisplayStyle.None;
        }

        _takeoffButton.enabledSelf = !_shipInRoute;
        _landingButton.enabledSelf = !string.IsNullOrEmpty(_selectedPlanetInfo.OutpostScene) && !_shipInRoute;
    }

    private void MoveToPlanet()
    {
        _spaceship.MoveTo(SelectedPlanetPresenter.ShipPoint.position);
        _playerSettings.CurrentPlanetID = _selectedPlanetInfo.ID;
        _playerSettings.SaveSettings();
        _shipInRoute = true;
        RefreshNavigationButtons();
    }

    private void PlanetLanding()
    {
        var landingScene = _selectedPlanetInfo.OutpostScene;
        if (!string.IsNullOrEmpty(landingScene))
        {
            _sceneNavigator.NavigationVector = NavigationVector.ReturnFromOrbit;
            SceneManager.LoadScene(landingScene);
        }
    }

    private void PlanetSelect(int id)
    {
        var selectedObject = _planetPresenters.First(x => x.ID == id);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedObject.transform.position);

        // Получаем текущий размер UI-элемента
        float width = 300;// _selectFrame.layout.width;
        float height = 190;// _selectFrame.layout.height;


        // Получаем логические координаты UI Toolkit
        Vector2 uiPosition = new Vector2(screenPos.x, Screen.height - screenPos.y);

        var scale = GetPanelScaling();

        uiPosition.x *= scale.x;
        uiPosition.y *= scale.y;

        // Позиция верхнего левого угла, чтобы центр совпал
        float left = uiPosition.x - width / 2f;
        float top = uiPosition.y - height / 2f;

        _selectFrame.style.left = left;
        _selectFrame.style.top = top;

        if (id < 0)
            id = -1;
        _selectedPlanetInfo = _planetsInfo.First(x => x.ID == id);
        _upperInfoPanel.dataSource = _selectedPlanetInfo;
        RefreshNavigationButtons();
    }

    private void OnDestroy()
    {
        foreach (PlanetPresenter planetPresenter in _planetPresenters)
        {
            planetPresenter.OnSelect -= PlanetSelect;
        }
        _takeoffButton.clicked -= MoveToPlanet;
        _landingButton.clicked -= PlanetLanding;
        _spaceship.HasArrived -= ShipArrived;
    }

}
