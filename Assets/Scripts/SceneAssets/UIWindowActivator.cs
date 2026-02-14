using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIWindowActivator : UIActivator
{
    [SerializeField]
    protected string _windowName;
    private VisualElement _window;
    private Button _closeButton;

    private void Start()
    {
        _window = _document.rootVisualElement.Q<VisualElement>(_windowName);
        _activateButton.clicked += ActivateWindow;

        _closeButton = _window.Query<Button>("CloseButton");
        _closeButton.clicked += DeactivateWindow;
    }

    private void ActivateWindow()
    {
        _window.style.visibility = Visibility.Visible;
    }

    private void DeactivateWindow()
    {
        _window.style.visibility = Visibility.Hidden;
    }

    private void OnDestroy()
    {
        _activateButton.clicked -= ActivateWindow;
        _closeButton.clicked -= DeactivateWindow;
    }
}
