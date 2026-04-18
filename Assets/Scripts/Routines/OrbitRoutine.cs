using Assets.Scripts.Orbit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OrbitRoutine : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [SerializeField]
    private List<PlanetPresenter> _planetPresenters;

    private VisualElement _upperInfoPanel;
    private Button _landingButton;
    private Image _selectFrame;


    private List<PlanetInfo> _planetsInfo;
    private PlanetInfo _selectedPlanetInfo;
    
    void Start()
    {
        _planetsInfo = Resources.Load<Planets>("Planets").Data;

        _upperInfoPanel = _document.rootVisualElement.Q<VisualElement>("UpperInfoPanel");
        _landingButton = _document.rootVisualElement.Q<Button>("LandingButton");
        _selectFrame = _document.rootVisualElement.Q<Image>("SelectFrame");

        foreach (PlanetPresenter planetPresenter in _planetPresenters)
        {
            planetPresenter.OnSelect += PlanetSelect;
        }

        _landingButton.clicked += PlanetLanding;

        PlanetSelect(1);
    }

    private void PlanetLanding()
    {
        var landingScene = _selectedPlanetInfo.OutpostScene;
        if (!string.IsNullOrEmpty(landingScene))
        { 
            SceneManager.LoadScene(landingScene);
        }
    }

    private void PlanetSelect(int id)
    {
        var selectedObject = _planetPresenters.First(x => x.ID == id);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedObject.transform.position);

        // Получаем текущий размер UI-элемента
        float width = _selectFrame.layout.width;
        float height = _selectFrame.layout.height;

        // Центр объекта в экранных координатах
        float centerX = screenPos.x;
        float centerY = Screen.height - screenPos.y; // инверсия Y

        // Позиция верхнего левого угла, чтобы центр совпал
        float left = centerX - width / 2f;
        float top = centerY - height / 2f;

        _selectFrame.style.left = left;
        _selectFrame.style.top = top;

        if (id < 0)
            id = -1;
        _selectedPlanetInfo = _planetsInfo.First(x => x.ID == id);
        _upperInfoPanel.dataSource = _selectedPlanetInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (PlanetPresenter planetPresenter in _planetPresenters)
        {
            planetPresenter.OnSelect -= PlanetSelect;
        }
        _landingButton.clicked -= PlanetLanding;
    }

}
