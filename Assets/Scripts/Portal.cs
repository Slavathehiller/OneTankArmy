using Assets.Scripts.SceneNavigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string _name;

    [SerializeField] 
    private string _sceneToLoadName;

    [SerializeField]
    private string _portalToGoName;

    [SerializeField]
    private NavigationVector _navigationVector;

    [SerializeField]
    private Transform _exitPoint;

    public string Name => _name;
    public Transform ExitPoint => _exitPoint;

    [Inject]
    private ISceneNavigator _sceneNavigator;
    public void LoadNextScene()
    {
        _sceneNavigator.NavigationVector = _navigationVector;
        _sceneNavigator.PortalToGo = _portalToGoName;
        SceneManager.LoadScene(_sceneToLoadName);
    }
}
