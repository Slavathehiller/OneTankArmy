using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _damage = 10;
    public float Damage => _damage;

    public void Explode()
    {
        Destroy(gameObject);
    }
}
