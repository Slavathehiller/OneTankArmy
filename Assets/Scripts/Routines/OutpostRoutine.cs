using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OutpostRoutine : MonoBehaviour
{
    [SerializeField]
    private Shuttle _shuttle;
    [SerializeField]
    private UIDocument _document;
    private Button _takeOffButtonButton;
    void Start()
    {
        _takeOffButtonButton = _document.rootVisualElement.Q<Button>("TakeOffButton");
        _takeOffButtonButton.clicked += TakeOff;
    }


    public void TakeOff()
    {
        _takeOffButtonButton.style.visibility = Visibility.Hidden;
        _shuttle.GetComponent<Collider2D>().enabled = false;
        _shuttle.TakeOff(()=> SceneManager.LoadScene(Scenes.BATTLE_SCENE));
    }

    private void OnDestroy()
    {
        _takeOffButtonButton.clicked -= TakeOff;
    }
}
