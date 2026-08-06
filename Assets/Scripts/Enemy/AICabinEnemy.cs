using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public abstract class AICabinEnemy : AIRangedEnemy
    {

        [SerializeField]
        private GameObject[] _cabins;

        [SerializeField]
        private float _gunsRotationSpeed = 2f;

        protected override void FixedUpdateActions()
        {
            if (_isDead || _target == null)
                return;
            base.FixedUpdateActions();
            FollowEnemy();
        }

        private void FollowEnemy()
        {
            if (_cabins.Length == 0)
                return;
            foreach (var cabin in _cabins)
            {
                Vector2 direction = _target.transform.position - cabin.transform.position;

                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                targetAngle -= 90;

                float currentAngle = cabin.transform.eulerAngles.z;

                float angle = Mathf.LerpAngle(currentAngle, targetAngle, _gunsRotationSpeed * Time.deltaTime);

                cabin.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
