using System;
using UnityEngine;


public class Tank
{
    private float _maxHealth = 100;
    private float _health;
    public event Action HealthChanges;

    private const string CURRENT_HEALTH = "CurrentHealth";

    public Tank()
    {
        LoadPrefs();
        if (Health <= 0)
            Health = _maxHealth;
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
            PlayerPrefs.SetFloat(CURRENT_HEALTH, _health);
            HealthChanges?.Invoke();
        }
    }

    public void LoadPrefs()
    {
        Health = PlayerPrefs.GetFloat(CURRENT_HEALTH);
    }

    public void TakeDamage(float damage)
    {
        if (Health > 0)
            Health -= damage;
    }
}

