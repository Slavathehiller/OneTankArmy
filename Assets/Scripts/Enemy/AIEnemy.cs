using Assets.Scripts.MISC;
using Assets.Scripts.VFX.Interfaces;
using System.Collections;
using System.Drawing;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;
using Zenject;

[System.Serializable]
public struct BodyParts
{
    public Rigidbody2D[] Parts;
}
public abstract class AIEnemy : BaseEntity
{
    [SerializeField]
    protected float _maxHP;
    protected override float MaxHP => _maxHP;

    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    protected NavMeshAgent _agent;

    [SerializeField]
    protected float _distanceOfDetection;

    [SerializeField]
    protected float _detectionCooldown;
    protected float _currentDetectionCooldown = 0;

    [SerializeField]
    protected float _meleeAttackCooldown;
    [SerializeField]
    protected float _meleeDistance;
    protected float _currentMeleeAttackCooldown;

    [SerializeField]
    protected float _meleeDamage;

    protected GameObject _target;

    public bool IsDead => _isDead;

    [SerializeField]
    protected GameObject _mainBody;

    [SerializeField]
    protected BodyParts[] _bodyPartsCollection;

    private int? _bodyPartIndex = null;

    private Collider2D _mainCollider;
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
        FixedUpdateActions();
    }

    protected virtual void FixedUpdateActions()
    {
        if (_agent != null && _agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance)
        {
            var angleToPoint = RotateCalculator.AngleTolookAt(transform, _agent.steeringTarget);
            _agent.isStopped = angleToPoint != null && Mathf.Abs(angleToPoint.Value) > 30;
           TryToRotateAt(_agent.steeringTarget);
        }
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

    public void SetAgentOn()
    {
        _agent.enabled = true;
    }

    protected virtual bool InContactWithTarget
    {
        get
        {
            return (_target != null
                && Vector3.Distance(transform.position, _target.transform.position) <= _meleeDistance
                && RotateCalculator.AngleTolookAt(transform, _target.transform.position) <= 15);
        }
    }

    public void GetMinimapMark(GameObject minimapMark)
    {
        minimapMark.transform.SetParent(_mainBody.transform);
        minimapMark.transform.localPosition = Vector3.zero;
    }

    protected virtual void StartActions() 
    {
        _currentHP = MaxHP;
        _mainCollider = GetComponent<Collider2D>();
        if (_agent != null)
        {
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
            _agent.speed = _moveSpeed;
            var currentZ = transform.eulerAngles.z;
            transform.eulerAngles = new Vector3(0f, 0f, currentZ);
        }
    }

    protected virtual bool TryToRotateAt(Vector3 point)
    {
        var angleToTarget = RotateCalculator.AngleTolookAt(transform, point);
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

    protected virtual bool TryToRotateAtTarget()
    {
        if (_target == null)
            return false;
        return TryToRotateAt(_target.transform.position);
    }

    protected void MoveToTarget()
    {
        if (IsDead)
            return;
        _animator.SetBool("Moving", true);
        _agent.SetDestination(_target.transform.position);
    }
    protected void StopMoving()
    {
        if (IsDead)
            return;
        _animator.SetBool("Moving", false);
        _agent.ResetPath();
    }

    protected virtual void DetectEnemy(TankController player)
    {
        if (IsDead || player.IsDead)
            return;
        _target = player.gameObject;
        _target.GetComponent<TankController>().Die += TagetDead;
    }

    protected virtual void LooseEnemy()
    {
        if (_target != null)
        {
            _target.GetComponent<TankController>().Die -= TagetDead;
            _target = null;
        }
    }

    private void TagetDead(BaseEntity target)
    {
        LooseEnemy();
        target.Die -= TagetDead;
    }

    public override void TakeDamage(float damage)
    {
        if (_isDead) return;
        base.TakeDamage(damage);
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
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
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
        base.CheckIfDead();
        if (_isDead)
            DeadPerfomance();
    }

    protected abstract void ReactToDamage(DamageDealer dd);
    protected virtual void DeadPerfomance()
    {
        if (_mainCollider != null)
            _mainCollider.enabled = false;
        if (_agent != null)
            _agent.enabled = false;
        Destroy(_mainBody);
        foreach (var bodyPart in BodyParts)
        {
            bodyPart.gameObject.SetActive(true);
        }
    }

    protected IEnumerator MakeGoooCoroutine<T>(GameObject mark, float scale = 1, UnityAction callback = null) where T : MonoBehaviour
    {
        yield return new WaitForSeconds(1f);
        var goo = _VFXMmanager.MeakeVFXAt<T>(mark.transform.position);
        goo.transform.localScale = new Vector3(scale, scale, 1);
        goo.transform.rotation = mark.transform.rotation;
        callback?.Invoke();
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
}
