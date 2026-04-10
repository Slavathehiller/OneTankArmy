using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatTooltip : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    private VisualElement _root;
    private Label _amount;
    private Image _icon;

    void Awake()
    {
        _root = _document.rootVisualElement.Q<VisualElement>("Root");
        _amount = _root.Q<Label>("Amount");
        _icon = _root.Q<Image>("Icon");
    }

    public void Show(Vector3 position, string message, Sprite iconSprite)
    {
        _amount.text = message;
        _icon.sprite = iconSprite;
        StopAllCoroutines();
        StartCoroutine(FloatCoroutine(position));
    }

    IEnumerator FloatCoroutine(Vector3 position)
    {
        var lifeTime = 0.5f;

       Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        _root.style.display = DisplayStyle.Flex;
        var currentPosition = screenPos;
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            currentPosition = new Vector3(currentPosition.x, currentPosition.y - 0.2f, currentPosition.z);
            _root.style.left = currentPosition.x;
            _root.style.top = currentPosition.y;
        }

        _root.style.display = DisplayStyle.None;
        Destroy(gameObject);
    }

}
