using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        protected GameObject _missilePrefab;

        protected override void UpdateActions()
        {
            if (_currentRangeAttackCooldown > 0)
                _currentRangeAttackCooldown -= Time.deltaTime;
            base.UpdateActions();
        }

        protected void Fire()
        {
            var missile = Instantiate(_missilePrefab);
            missile.transform.position = _firePoint.transform.position;
            missile.transform.rotation = _firePoint.transform.rotation;
            missile.SetActive(true);
        }
    }
}
