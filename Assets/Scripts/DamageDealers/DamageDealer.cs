using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] protected float _damage = 5;
    public float Damage => _damage;

    public virtual void ReactToCollision(GameObject collision)
    {
        Destroy(gameObject);
    }

}
