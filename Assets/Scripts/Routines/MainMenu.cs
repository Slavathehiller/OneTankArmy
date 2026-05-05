using Assets.Player;
using Assets.Scripts.Orbit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [SerializeField]
    private AudioSource _audioSource;

    private Button _startButton;
    private Button _exitButton;

    private List<Button> _allButtons;

    [Inject]
    private IPlayerSettings _playerSettings;
    private void Awake()
    {
        _allButtons = _document.rootVisualElement.Query<Button>().ToList();
        foreach (Button button in _allButtons)
        {
            button.clicked += SoundPlay;
        }

        _startButton = _document.rootVisualElement.Q("PlayButton") as Button;
        _exitButton = _document.rootVisualElement.Q("ExitButton") as Button;
        _startButton.clicked += StartGame;
        _exitButton.clicked += ExitGame;
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        var planetsInfo = Resources.Load<Planets>("Planets").Data;
        var currentPlanetInfo = planetsInfo.First(x => x.ID == _playerSettings.CurrentPlanetID);
        if (!string.IsNullOrEmpty(currentPlanetInfo.OutpostScene))
            SceneManager.LoadScene(currentPlanetInfo.OutpostScene);
        else
            SceneManager.LoadScene(Scenes.ORBIT_SCENE);
    }

    private void SoundPlay()
    {
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        _startButton.clicked -= StartGame;
        _exitButton.clicked -= ExitGame;

        foreach (Button button in _allButtons)
        {
            button.clicked -= SoundPlay;
        }
    }
}
