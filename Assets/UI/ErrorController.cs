using UnityEngine;
using UnityEngine.UIElements;

public class ErrorController : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;
    private VisualElement _errorWindow;
    private Label _errorLabel;
    private Button _closeButton;
    void Start()
    {
        _errorWindow = _document.rootVisualElement.Q<VisualElement>("ErrorWindow");
        _errorLabel = _errorWindow.Q<Label>("ErrorLabel");
        _closeButton = _errorWindow.Q<Button>("CloseButton");
        _closeButton.clicked += CloseWindow;
    }

    public void ShowError(string error)
    {
        _errorLabel.text = error;
        _errorWindow.style.display = DisplayStyle.Flex;
    }

    private void CloseWindow()
    {
        _errorWindow.style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        _closeButton.clicked -= CloseWindow;
    }

}
