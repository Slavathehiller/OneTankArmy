using Assets.Scripts.DamageDealers;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEntity : MonoBehaviour
{
    protected abstract float MaxHP { get; }

    protected float _currentHP;

    public event UnityAction<BaseEntity> Die;

    protected bool _isDead;

    [SerializeField]
    protected float _moveSpeed = 5;
    [SerializeField]
    protected float _rotateSpeed = 100;

    [SerializeField]
    protected GameObject[] _injuries;

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

    public virtual void TakeDamage(float damage)
    {
        var nextHPpercentage = (_currentHP - damage) / MaxHP;
        var activeInjuriesTobe = 0;
        if (nextHPpercentage <= 0.75f)
            activeInjuriesTobe = 1;
        if (nextHPpercentage <= 0.50f)
            activeInjuriesTobe = 2;
        if (nextHPpercentage <= 0.25f)
            activeInjuriesTobe = 3;

        var injuriesToApply = activeInjuriesTobe - _injuries.Count(x => x.activeSelf);
        var inactiveInjuries = _injuries.Where(x => !x.activeSelf).ToArray();
        while (injuriesToApply > 0 && inactiveInjuries.Count() > 0)
        {
            var injuryIndex = Random.Range(0, inactiveInjuries.Count());
            inactiveInjuries[injuryIndex].SetActive(true);
            inactiveInjuries = inactiveInjuries.Where(x => !x.activeSelf).ToArray();
            injuriesToApply--;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealerDOT>(out var ddDOT))
        {
            TakeDamage(ddDOT.DOT);
        }
    }

}
