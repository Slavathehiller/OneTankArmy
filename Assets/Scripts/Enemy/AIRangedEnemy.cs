using Assets.Scripts.DamageDealers;
using Assets.Scripts.ObjectPool;
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemy
{
    public abstract class AIRangedEnemy : AIEnemy
    {
        [SerializeField]
        protected float _distanceOfRangeAttack;

        [SerializeField]
        protected float _rangeAttackCooldown;
        protected float _currentRangeAttackCooldown = 0;

        [SerializeField]
        protected GameObject _firePoint;

        [SerializeField]
        protected MissileType _missileType;
        
        [SerializeField]
        protected AudioSource _missileSound;

        [Inject]
        protected IMissilePool _missilePool;

        protected abstract void RangedAttack();
        protected override void StartActions()
        {
           // _agent.stoppingDistance = _distanceOfRangeAttack;
            base.StartActions();
        }

        protected override void UpdateActions()
        {
            if (_currentRangeAttackCooldown > 0)
                _currentRangeAttackCooldown -= Time.deltaTime;
            base.UpdateActions();
        }


        protected override void FixedUpdateActions()
        {
            base.FixedUpdateActions();
            if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfRangeAttack)
            {
                if (HasLineOfSight(_target))
                    StopMoving();

                if (_currentRangeAttackCooldown <= 0 && HasLineOfSight(_target))
                    RangedAttack();
            }
        }


        protected virtual bool HasLineOfSight(GameObject target)
        {
            int layerMask = 4 + 128 + 256;  //Ignore Raycast + Missile + PlayerMissile
            layerMask = ~layerMask;

            RaycastHit2D hit = Physics2D.Raycast(_firePoint.transform.position, transform.up, _distanceOfRangeAttack, layerMask);
            //Debug.DrawRay(_firePoint.transform.position, transform.up * 5, Color.yellow, 1);

            return (hit && hit.collider.gameObject.TryGetComponent<PlayerSide>(out var hited) && hited.gameObject == target);
        }

        protected override void DetectEnemy(PlayerSide player)
        {
            if (IsDead || player.IsDead)
                return;
            base.DetectEnemy(player);
            if(Vector3.Distance(transform.position, _target.transform.position) > _distanceOfRangeAttack || !HasLineOfSight(_target))
                MoveToTarget();
        }

        protected override void LooseEnemy()
        {
            base.LooseEnemy();
            StopMoving();
        }

        protected virtual void Fire()
        {
            var missile = _missilePool.GetMissile(_missileType);
            missile.transform.position = _firePoint.transform.position;
            missile.transform.rotation = _firePoint.transform.rotation;
            missile.Init();
            if (_missileSound  != null)
                _missileSound.Play();
        }
    }
}
