using Assets.Player;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100;
    private float _health;
    public event UnityAction HealthChanges;

    [SerializeField]
    private TankController _tankController;

    public TankController TankController
    {
        get
        {
            if (_tankController == null)
                _tankController = GetComponent<TankController>();
            return _tankController;
        }
    }

    [Inject]
    private IPlayerSettings _playerSettings;


    private void Start()
    {
        Health = _playerSettings.CurrentHealth;
        if (Health == float.MinValue)
            Health = MaxHealth;
    }
    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            HealthChanges?.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        if (Health > 0)
            Health -= damage;
    }

    public void ControlOff()
    {

        TankController.ControlOff();
    }

    public void EvacuateFlareOn()
    {
        TankController.EvacuateFlareOn();
    }
}
