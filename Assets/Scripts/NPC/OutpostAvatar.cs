using Assets.Scripts.Factories;
using Assets.Scripts.NPC;
using Assets.Vehicles;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OutpostAvatar : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SimpleMover _mover;
    private float _health;

    public float Health 
    {  
        get
        { 
            return _health; 
        } 
        set 
        { 
            _health = value; 
        } 
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mover = GetComponent<SimpleMover>();

    }

    public void SetVechicleType(VehicleType newType)
    {
        var prefabPath = PrefabsPath.GetPathForVehicle(newType);
        var prefab = Resources.Load<GameObject>(prefabPath);
        _health = prefab.GetComponent<Vehicle>().MaxHealth;
        var presenter = Resources.Load<VehiclePresenters>("VehiclePresenters").Data.First(x => x.VehicleType == newType);

        _spriteRenderer.sprite = presenter.Portrait;
    }

    public void MoveTo(Vector3 movePoint, float moveSpeed = 1, UnityAction callback= null)
    {
        _mover.MoveTo(movePoint, moveSpeed, callback);
    }
}
