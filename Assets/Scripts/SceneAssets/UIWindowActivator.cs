using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIWindowActivator : UIActivator
{
    public event Action OnShowWindow;
    [SerializeField]
    protected string _windowName;
    private VisualElement _window;
    private Button _closeButton;

    public Func<bool> WindowAvailable;

    private void Start()
    {
        _window = _document.rootVisualElement.Q<VisualElement>(_windowName);
        _activateButton.clicked += ActivateWindow;

        _closeButton = _window.Query<Button>("CloseButton");
        _closeButton.clicked += DeactivateWindow;
    }

    protected void ActivateWindow()
    {
        if (WindowAvailable == null || WindowAvailable())
        {
            _window.style.visibility = Visibility.Visible;
            OnShowWindow?.Invoke();
        }
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
