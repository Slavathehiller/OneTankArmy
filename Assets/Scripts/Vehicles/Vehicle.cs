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
        GetComponent<TankController>().ControlOff();
    }
}
