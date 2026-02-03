using Assets.Scripts.MISC;
using UnityEngine;

public class GiantScolopendra : AIEnemy
{

    [SerializeField]
    protected float _distanceOfAttack;

    [SerializeField]
    protected float _meleeAttackCooldown;
    protected float _currentMeleeAttackCooldown;


    protected override void UpdateActions()
    {
        _currentMeleeAttackCooldown -= Time.deltaTime;
        base.UpdateActions();
    }

    private void FixedUpdate()
    {

        if (_isDead)
            return;

        if (_target == null)
        {
            _animator.SetBool("Moving", false);
            return;
        }

        if (!TryToRotateAtTarget())
        {
            RigidBody.angularVelocity = 0;
            if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfAttack)
            {
                if (_currentMeleeAttackCooldown <= 0)
                    Bite();
            }
            else
            {
                MoveToTarget();
            }
        }
    }

    protected override bool TryToRotateAtTarget()
    {
        var angleToTarget = RotateCalculator.AngleTolookAt(transform, _target.transform);
        var minAngleToCurve = 40;
        if (angleToTarget != null)
        {
            if (Mathf.Abs(angleToTarget.Value) > 5)
            {
                _animator.SetBool("Moving", true);
                if (angleToTarget < 0)
                {
                    _animator.SetBool("TurningLeft", false);
                    _animator.SetBool("TurningRight", angleToTarget <= -minAngleToCurve);
                    RigidBody.AddTorque(-_rotateSpeed);
                }
                else
                {
                    _animator.SetBool("TurningRight", false);
                    _animator.SetBool("TurningLeft", angleToTarget >= minAngleToCurve);
                    RigidBody.AddTorque(_rotateSpeed);
                }
                return true;
            }

            if (Mathf.Abs(angleToTarget.Value) < minAngleToCurve && Vector3.Distance(transform.position, _target.transform.position) >= _distanceOfAttack)
            {
                RigidBody.AddForce(transform.up * _moveSpeed);
                _animator.SetBool("TurningLeft", false);
                _animator.SetBool("TurningRight", false);
                _animator.SetBool("Moving", Mathf.Abs(angleToTarget.Value) < minAngleToCurve);
            }

        }
        _animator.SetBool("TurningLeft", false);
        return false;
    }

    protected void Bite()
    {
        _animator.SetTrigger("Bite");
        if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfAttack)
            _target.GetComponent<TankController>().TakeDamage(_meleeDamage);
        _currentMeleeAttackCooldown = _meleeAttackCooldown;
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
        StartCoroutine(MakeGoooCoroutine<ToxicGoo>(BodyParts[0].gameObject, DisablePhysic));
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        
    }
}
