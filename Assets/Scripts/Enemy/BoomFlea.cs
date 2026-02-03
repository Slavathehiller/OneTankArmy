using Assets.Scripts.VFX.Interfaces;
using System;
using UnityEngine;
using Zenject;

public class BoomFlea : AIEnemy
{
    [SerializeField]
    private float _distanceOfJump;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private float _jumpMaxTime;
   
    private float _jumpTime;
    private float _jumpCooldown;

    [Inject]
    private IVFXManager _VFXMmanager;

    private bool IsJumping => _jumpTime > 0;

    private void FixedUpdate()
    {
        if (_target == null)
        {
            _animator.SetBool("Moving", false);
            return;
        }

        if (IsJumping)
        {
            RigidBody.AddForce(transform.up * _jumpSpeed);
            return;
        }
        else
            _animator.SetBool("Jumping", false);


        if (!TryToRotateAtTarget())
        {
            RigidBody.angularVelocity = 0;
            if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfJump)
            {
                StopMoving();
                if (_jumpCooldown <= 0)
                    Jump();
            }
            else
                MoveToTarget();
        }           
    }

    protected override void UpdateActions()
    {
        _jumpTime -= Time.deltaTime;
        _jumpCooldown -= Time.deltaTime;
        base.UpdateActions();
    }

    private void Jump()
    {
        _jumpTime = _jumpMaxTime;
        _jumpCooldown = _jumpTime * 2;
        _animator.SetBool("Jumping", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReactToDamage(null);
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        _currentHP = 0;
        CheckIfDead();
    }

    protected override void DeadPerfomance()
    {
        Explode();
    }

    private void Explode()
    {
        _VFXMmanager.MeakeExplosionAt(transform.position);
        Destroy(gameObject);
    }
}
