using Assets.Scripts.MISC;
using UnityEngine;

public class GiantScolopendra : AIEnemy
{
    protected override void UpdateActions()
    {
        _currentMeleeAttackCooldown -= Time.deltaTime;
        base.UpdateActions();
    }

    protected override void FixedUpdateActions()
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
            if (InContactWithTarget)
            {
                if (_currentMeleeAttackCooldown <= 0)
                    Bite();
            }
        }
    }

    protected override void DetectEnemy(TankController player)
    {
        if (IsDead && player.IsDead)
            return;
        base.DetectEnemy(player);
        if (!InContactWithTarget)
            MoveToTarget();
    }

    protected override void LooseEnemy()
    {
        base.LooseEnemy();
        StopMoving();
    }


    protected override bool TryToRotateAt(Vector3 point)
    {
        var angleToTarget = RotateCalculator.AngleTolookAt(transform, point);
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

            //if (Mathf.Abs(angleToTarget.Value) < minAngleToCurve && !_inContactWithTarget)
            //{
            //    RigidBody.AddForce(transform.up * _moveSpeed);
            //    _animator.SetBool("TurningLeft", false);
            //    _animator.SetBool("TurningRight", false);
            //    _animator.SetBool("Moving", Mathf.Abs(angleToTarget.Value) < minAngleToCurve);
            //}

        }
        _animator.SetBool("TurningLeft", false);
        return false;
    }

    protected void Bite()
    {
        _animator.SetTrigger("Bite");
        StopMoving();
        _currentMeleeAttackCooldown = _meleeAttackCooldown;
    }

    protected void EndBiting()
    {
        if (InContactWithTarget)
            _target.GetComponent<TankController>().TakeDamage(_meleeDamage);
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
        StartCoroutine(MakeGoooCoroutine<ToxicGoo>(BodyParts[0].gameObject, 1, DisablePhysic));
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        
    }
}
