using UnityEngine;
using UnityEngine.UIElements;

public class UIActivator : MonoBehaviour
{
    [SerializeField]
    protected UIDocument _document;

    [SerializeField]
    protected string _buttonName;

    protected Button _activateButton;

    void Awake()
    {
        _activateButton = _document.rootVisualElement.Q<Button>(_buttonName);
    }

    private void OnMouseEnter()
    {
        _activateButton.style.visibility = Visibility.Visible;
    }

    private void OnMouseExit()
    {
        _activateButton.style.visibility = Visibility.Hidden;
    }
}
