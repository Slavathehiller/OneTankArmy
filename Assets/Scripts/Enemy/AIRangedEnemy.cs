using Assets.Scripts.DamageDealers;
using Assets.Scripts.ObjectPool;
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

        [Inject]
        private IMissilePool _missilePool;

        protected override void UpdateActions()
        {
            if (_currentRangeAttackCooldown > 0)
                _currentRangeAttackCooldown -= Time.deltaTime;
            base.UpdateActions();
        }

        protected void Fire()
        {
            var missile = _missilePool.GetMissile(_missileType);
            missile.transform.position = _firePoint.transform.position;
            missile.transform.rotation = _firePoint.transform.rotation;
            missile.Init();
        }
    }
}
