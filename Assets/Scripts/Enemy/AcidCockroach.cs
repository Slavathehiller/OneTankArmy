using Assets.Scripts.Enemy;
using Assets.Scripts.VFX.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

[System.Serializable]
public struct BodyParts
{
    public Rigidbody2D[] Parts;
}

public class AcidCockroach : AIRangedEnemy
{
    private float _shokedTime;
    protected bool IsShocked => _shokedTime > 0;

    private void FixedUpdate()
    {

        if (_isDead)
                return;

        if (_target == null)
        {
            _animator.SetBool("Moving", false);
            return;
        }

        if (IsShocked)
            return;

        if (!TryToRotateAtTarget())
        {
            RigidBody.angularVelocity = 0;
            if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfRangeAttack)
            {
                StopMoving();
                if (_currentRangeAttackCooldown <= 0)
                {
                    AcidSpit();
                    _currentRangeAttackCooldown = _rangeAttackCooldown;
                }
            }
            else
            {
                if (!IsShocked)
                    MoveToTarget();
            }
        }
        else
            _animator.SetBool("Moving", false);
    }

    protected override void UpdateActions()
    {
        _shokedTime -= Time.deltaTime;
        base.UpdateActions();
    }

    private void AcidSpit()
    {
        if (_target != null && !_isDead) 
        {
            _animator.SetTrigger("AcidSpit");
        }
    }

    public void AcidSpitStart()
    {
        if (!_isDead)
            Fire();
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        if (_shokedTime <= 0)
        {
            _shokedTime = 1;
            _animator.SetTrigger("Shoke");
        }
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
        StartCoroutine(MakeGoooCoroutine<AcidGoo>(BodyParts[0].gameObject, DisablePhysic));
    }

}
