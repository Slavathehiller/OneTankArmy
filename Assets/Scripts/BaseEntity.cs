using Assets.Scripts.DamageDealers;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEntity : MonoBehaviour
{
    [SerializeField]
    protected float _maxHP;
    protected float _currentHP;

    public event UnityAction<BaseEntity> Die;

    protected bool _isDead;

    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected float _rotateSpeed = 100;

    private Rigidbody2D _rigidBody;

    protected Rigidbody2D RigidBody
    {
        get
        {
            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody2D>();
            return _rigidBody;
        }
    }

    protected virtual bool CheckHPOver()
    {
        return _currentHP <= 0;
    }
    protected virtual void CheckIfDead()
    {
        if (CheckHPOver())
        {
            _isDead = true;
            Die?.Invoke(this);
        }
    }

    public abstract void TakeDamage(float damage);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealerDOT>(out var ddDOT))
        {
            TakeDamage(ddDOT.DOT);
        }
    }

}
