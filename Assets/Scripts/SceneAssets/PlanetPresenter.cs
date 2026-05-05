using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlanetPresenter : MonoBehaviour, IPointerDownHandler
{
    public event UnityAction<int> OnSelect;
    [SerializeField]
    private int _id;

    [SerializeField]
    private Transform _shipPoint;
    [SerializeField]
    private Transform _namePoint;

    public int ID => _id;
    public Transform ShipPoint => _shipPoint;
    public Transform NamePoint => _namePoint;

    public void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log($"✅ Planet {ID} clicked! PointerDown сработал.");
        OnSelect?.Invoke(ID);
    }
}
