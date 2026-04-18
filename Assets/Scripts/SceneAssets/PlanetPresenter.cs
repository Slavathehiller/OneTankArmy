using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlanetPresenter : MonoBehaviour, IPointerDownHandler
{
    public event UnityAction<int> OnSelect;

    [SerializeField]
    public int ID;

    public void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log($"✅ Planet {ID} clicked! PointerDown сработал.");
        OnSelect?.Invoke(ID);
    }
}
