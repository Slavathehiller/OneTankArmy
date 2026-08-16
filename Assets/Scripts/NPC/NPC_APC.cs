using Assets.Scripts.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    public class NPC_APC : NPC_ConvoyMember
    {
        [SerializeField]
        protected float _distanceOfDetection = 5;

        [SerializeField]
        protected float _detectionCooldown = 1;
        protected float _currentDetectionCooldown = 0;


        [SerializeField]
        private Gun[] _guns;

        private TurretController _turret;
       
        protected override void StartActions()
        {
            base.StartActions();
            _turret = GetComponent<TurretController>();
        }

        protected override void UpdateActions()
        {
            base.UpdateActions();
            if (_currentDetectionCooldown > 0)
                _currentDetectionCooldown -= Time.deltaTime;
            else
            {
                var detectedTarget = TryDetect<AIEnemy>(_distanceOfDetection);
                if (detectedTarget != null)
                    _turret.BindTarget(detectedTarget.gameObject);
                else
                    _turret.UnbindTarget();

                _currentDetectionCooldown = _detectionCooldown;
            }
            if (_turret.CanFire())
                FireAtTarget();
        }

        private void FireAtTarget()
        {
            foreach (var gun in _guns)
                gun.TryFire();
        }

        protected T TryDetect<T>(float radius) where T : class
        {
            var detectedColliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in detectedColliders)
            {
                if (collider.gameObject.TryGetComponent<T>(out var target))
                {
                    return target;
                }
            }
            return null;
        }


    }
}
