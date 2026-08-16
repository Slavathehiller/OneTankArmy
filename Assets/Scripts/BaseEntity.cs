using Assets.Scripts.DamageDealers;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEntity : MonoBehaviour
{

    [SerializeField]
    protected AudioSource _audioSourceMove;

    [SerializeField]
    protected AudioSource _audioSourceDeath;
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

    [SerializeField]
    private Rigidbody2D _rigidBody;

    [SerializeField]
    protected GameObject _mainBody;

    [SerializeField]
    protected BodyParts[] _bodyPartsCollection;

    private int? _bodyPartIndex = null;

    protected Rigidbody2D[] BodyParts
    {
        get
        {
            if (_bodyPartIndex == null)
                _bodyPartIndex = Random.Range(0, _bodyPartsCollection.Length);
            if (_bodyPartsCollection.Length > 0)
                return _bodyPartsCollection[_bodyPartIndex.Value].Parts;
            else
                return new Rigidbody2D[0];
        }
    }

    public virtual bool IsDead => _isDead;

    private TagCloud _tagCloud;
    public TagCloud TagCloud => _tagCloud;

    private Collider2D _mainCollider;

    protected Rigidbody2D RigidBody
    {
        get
        {
            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody2D>();
            return _rigidBody;
        }
    }

    private void Start()
    {
        StartActions();
    }

    private void Update()
    {
        UpdateActions();
    }

    private void FixedUpdate()
    {
        if (_audioSourceMove != null)
        {
            if (RigidBody.linearVelocity.magnitude > 0.6f)
            {
                if (!_audioSourceMove.isPlaying)
                {
                    _audioSourceMove.Play();
                }
            }
            else
            {
                _audioSourceMove.Stop();
            }
        }

        FixedUpdateActions();
    }

    protected virtual void StartActions() 
    {
        InitTagCloud();
    }
    protected virtual void UpdateActions() 
    {
        _mainCollider = GetComponent<Collider2D>();
    }
    protected virtual void FixedUpdateActions() { }

    protected virtual void InitTagCloud()
    {
        _tagCloud = new();
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
            if (_audioSourceDeath != null)
                _audioSourceDeath.Play();
            Die?.Invoke(this);
            DeadPerfomance();
        }
    }

    public void InstantDeath()
    {
        _currentHP = 0;
        CheckIfDead();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
        {
            TakeDamage(dd.Damage);
            ReactToDamage(dd);
            var missile = dd as Missile;
            if (missile != null)
                missile.Remove();
        }
    }

    public void ForcedMove(Vector3 linearForce, float? angularForce = null)
    {
        RigidBody.AddForce(linearForce);
        if (angularForce != null)
            RigidBody.AddTorque(angularForce.Value);
    }


    protected virtual void DeadPerfomance()
    {
        if (_mainCollider != null)
            _mainCollider.enabled = false;
        Destroy(_mainBody);
        foreach (var bodyPart in BodyParts)
        {
            bodyPart.gameObject.SetActive(true);
        }
    }

    protected void DisablePhysic()
    {
        if (RigidBody != null)
            RigidBody.simulated = false;
        foreach (var bodyPart in BodyParts)
        {
            var bpRigidBody = bodyPart.GetComponent<Rigidbody2D>();
            if (bpRigidBody != null)
                bpRigidBody.simulated = false;
            var bpCollider = bodyPart.GetComponent<Collider2D>();
            if (bpCollider != null)
                bpCollider.enabled = false;
        }
        enabled = false;
    }


    protected virtual void ReactToDamage(DamageDealer dd) { }

}
