using UnityEngine;
using UnityEngine.Events;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100;
    private float _health;
    public event UnityAction HealthChanges;

    private const string CURRENT_HEALTH = "CurrentHealth";

    void Awake()
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
