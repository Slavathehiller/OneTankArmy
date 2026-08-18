using Assets.Scripts.ObjectPool;
using Assets.Scripts.Player;
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

    protected override void StartActions()
    {
        _agent.stoppingDistance = _distanceOfJump;
        base.StartActions();
    }

    protected override void FixedUpdateActions()
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

        if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfJump)
        {
            StopMoving();
            if (_jumpCooldown <= 0 && !TryToRotateAtTarget())
                Jump();
        }
    
        base.FixedUpdateActions();
    }

    protected override void UpdateActions()
    {
        _jumpTime -= Time.deltaTime;
        _jumpCooldown -= Time.deltaTime;
        base.UpdateActions();
    }

    protected override void InitTagCloud()
    {
        base.InitTagCloud();
        TagCloud.Add(Tag.Insect);
    }

    protected override void DetectEnemy(PlayerSide player)
    {
        if (player.IsDead)
            return;
        base.DetectEnemy(player);
        MoveToTarget();
    }

    protected override void LooseEnemy()
    {
        base.LooseEnemy();
        StopMoving();
    }

    private void Jump()
    {
        _jumpTime = _jumpMaxTime;
        _jumpCooldown = _jumpTime * 2;
        _animator.SetBool("Jumping", true);
        _agent.isStopped = true;
    }

    protected override void ReactToCollision()
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
        _VFXMmanager.MakeExplosionAt(transform.position);
        Destroy(gameObject);
    }
}
