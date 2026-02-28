using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolTipController : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;
    [SerializeField]
    private Vector2 _offset;

    private VisualElement _root;
    private VisualElement _tooltip;
    private Label _tooltipLabel;
    void Start()
    {
        _root = _document.rootVisualElement;
        _tooltip = _root.Q<VisualElement>("tooltip-panel");
        _tooltipLabel = _tooltip.Q<Label>("tooltip-label");

        _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _root.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnDestroy()
    {
        _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _root.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        _root.RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnPointerMove(PointerMoveEvent ev)
    {
        if(ev.target is not VisualElement hovered)
        {
            HideTooltip();
            return;
        }
        var text = hovered.tooltip;
        if (string.IsNullOrEmpty(text))
        {
            HideTooltip();
            return;
        }

        ShowTooltip(text, (Vector2)ev.position + _offset);
    }

    private void OnPointerLeave(PointerLeaveEvent ev)
    {
        HideTooltip();
    }

    private void OnPointerDown(PointerDownEvent ev)
    {
        HideTooltip();
    }

    private void ShowTooltip(string text, Vector2 position)
    {
        _tooltipLabel.text = text;
        _tooltip.style.left = position.x;
        _tooltip.style.top = position.y; 
        _tooltip.style.display = DisplayStyle.Flex;
    }

    private void HideTooltip()
    {
        _tooltip.style.display = DisplayStyle.None;
    }


}
