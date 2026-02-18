using Assets.Scripts.MISC;
using Assets.Scripts.VFX.Interfaces;
using System.Collections;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class AIEnemy : BaseEntity
{
    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    protected float _maxHP;
    protected float _currentHP;

    [SerializeField]
    protected float _distanceOfDetection;

    [SerializeField]
    protected float _detectionCooldown;
    protected float _currentDetectionCooldown = 0;

    [SerializeField]
    protected float _meleeAttackCooldown;
    protected float _currentMeleeAttackCooldown;

    [SerializeField]
    protected float _meleeDamage;

    protected GameObject _target;

    protected bool _inContactWithTarget;

    protected bool _isDead;

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
            return _bodyPartsCollection[_bodyPartIndex.Value].Parts;
        }
    }

    [Inject]
    private IVFXManager _VFXMmanager;

    void Start()
    {
        StartActions();
    }

   // Update is called once per frame
    void Update()
    {
        UpdateActions();
    }

    protected virtual void UpdateActions()
    {
        if (_currentDetectionCooldown > 0)
            _currentDetectionCooldown -= Time.deltaTime;
        if (_currentMeleeAttackCooldown > 0)
            _currentMeleeAttackCooldown -= Time.deltaTime;
        if (_currentDetectionCooldown <= 0)
        {
            var detectedColliders = Physics2D.OverlapCircleAll(transform.position, _distanceOfDetection);
            TankController enemyFound = null;
            foreach (Collider2D collider in detectedColliders)
            {
                if (collider.gameObject.TryGetComponent<TankController>(out var tank))
                {
                    enemyFound = tank;
                    break;
                }
            }
            if (enemyFound)
                DetectEnemy(enemyFound);
            else
                LooseEnemy();

            _currentDetectionCooldown = _detectionCooldown;
        }
    }

    protected virtual void StartActions() 
    {
        _currentHP = _maxHP;
    }

    protected virtual bool TryToRotateAtTarget()
    {
        var angleToTarget = RotateCalculator.AngleTolookAt(transform, _target.transform);
        if (angleToTarget != null)
        {
            if (Mathf.Abs(angleToTarget.Value) > 5)
            {
                _animator.SetBool("Moving", true);
                if (angleToTarget < 0)
                    RigidBody.AddTorque(-_rotateSpeed);
                else
                    RigidBody.AddTorque(_rotateSpeed);
                return true;
            }
        }
        return false;
    }

    protected void MoveToTarget()
    {
        _animator.SetBool("Moving", true);
        RigidBody.AddForce(transform.up * _moveSpeed);
    }
    protected void StopMoving()
    {
        RigidBody.linearVelocity = Vector3.zero;
        _animator.SetBool("Moving", false);
    }

    protected virtual void DetectEnemy(TankController controller)
    {
        if (controller.IsDead)
            return;
        _target = controller.gameObject;
        _target.GetComponent<TankController>().Die += TagetDead;
    }

    private void TagetDead()
    {
        LooseEnemy();
    }

    protected virtual void LooseEnemy()
    {
        if (_target != null)
        {
            _target.GetComponent<TankController>().Die -= TagetDead;
            _target = null;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (_isDead) return;
        _currentHP -= damage;
        CheckIfDead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
        {
            TakeDamage(dd.Damage);
            ReactToDamage(dd);
            dd.gameObject.SetActive(false);
        }
        if (collision.gameObject == _target)
        {
            _inContactWithTarget = true;
        }
        //ReactToCollision(collision);
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == _target)
        {
            _inContactWithTarget = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
        {
            TakeDamage(dd.Damage);
            ReactToDamage(dd);
            dd.ReactToCollision(gameObject);
        }
    }

    protected override void CheckIfDead()
    {
        if (_currentHP <= 0)
        {
            _isDead = true;
            DeadPerfomance();
        }        
    }

   // protected abstract void ReactToCollision(Collision2D collision);
    protected abstract void ReactToDamage(DamageDealer dd);
    protected virtual void DeadPerfomance()
    {
        Destroy(_mainBody);
        foreach (var bodyPart in BodyParts)
        {
            bodyPart.gameObject.SetActive(true);
        }
    }

    protected IEnumerator MakeGoooCoroutine<T>(GameObject mark, UnityAction callback = null) where T : MonoBehaviour
    {
        yield return new WaitForSeconds(1f);
        var goo = _VFXMmanager.MeakeVFXAt<T>(mark.transform.position);
        goo.transform.rotation = mark.transform.rotation;
        callback?.Invoke();
    }

    protected void DisablePhysic()
    {
        var mainCollider = GetComponent<Collider2D>();
        if (mainCollider != null)
            mainCollider.enabled = false;
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

}
