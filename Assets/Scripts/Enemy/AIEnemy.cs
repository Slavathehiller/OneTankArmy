using Assets.Scripts.MISC;
using Assets.Scripts.Player;
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

    protected override void FixedUpdateActions()
    {
        base.FixedUpdateActions();
        if (_agent != null && _agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance)
        {
            var angleToPoint = RotateCalculator.AngleTolookAt(transform, _agent.steeringTarget);
            _agent.isStopped = angleToPoint != null && Mathf.Abs(angleToPoint.Value) > 30;
           TryToRotateAt(_agent.steeringTarget);
        }
    }

    protected override void UpdateActions()
    {
        base.UpdateActions();
        if (_currentDetectionCooldown > 0)
            _currentDetectionCooldown -= Time.deltaTime;
        if (_currentMeleeAttackCooldown > 0)
            _currentMeleeAttackCooldown -= Time.deltaTime;

        if (_currentDetectionCooldown <= 0)
        {

            PlayerSide enemyFound = TryDetect(_distanceOfDetection);
            if (enemyFound)
                DetectEnemy(enemyFound);
            else
                Invoke("LooseEnemy", 2);

            _currentDetectionCooldown = _detectionCooldown;
        }
    }

    protected PlayerSide TryDetect(float radius)
    {
        var detectedColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in detectedColliders)
        {
            if (collider.gameObject.TryGetComponent<PlayerSide>(out var target))
            {
                return target;
            }
        }
        return null;
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

    protected override void StartActions() 
    {
        base.StartActions();
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
        //if (_audioSourceMove != null && !_audioSourceMove.isPlaying)            
        //    _audioSourceMove.Play();
    }
    protected void StopMoving()
    {
        if (IsDead)
            return;
        _animator.SetBool("Moving", false);
        _agent.ResetPath();
        if (_audioSourceMove != null)
            _audioSourceMove.Stop();
    }

    protected virtual void DetectEnemy(PlayerSide player)
    {
        if (IsDead || player.IsDead)
            return;
        _target = player.gameObject;
        _target.GetComponent<PlayerSide>().Die += TagetDead;
    }

    protected virtual void LooseEnemy()
    {
        if (_target != null && Vector3.Distance(transform.position, _target.transform.position) > _distanceOfDetection * 1.2f)
        {
            var playerSide = _target.GetComponent<PlayerSide>();
            if (playerSide != null)
            {
                playerSide.Die -= TagetDead;
                _target = null;
            }
        }
    }

    private void TagetDead(BaseEntity target)
    {
        if (_isDead) return;
        LooseEnemy();
        target.Die -= TagetDead;
    }

    public override void TakeDamage(float damage)
    {
        if (_isDead) return;
        base.TakeDamage(damage);
        _currentHP -= damage;
        CheckIfDead();
        if (!_isDead)
        {
            var target = TryDetect(_distanceOfDetection * 2);
            if (target != null)
                DetectEnemy(target);
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
        base.CheckIfDead();
        if (_isDead)
            DeadPerfomance();
    }

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
