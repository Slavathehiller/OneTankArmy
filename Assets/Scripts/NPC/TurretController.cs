using Assets.Scripts.MISC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _cabins;

        [SerializeField]
        private float _gunsRotationSpeed = 10;

        private Transform _target;

        public void BindTarget(GameObject target)
        {
            _target = target.transform;
        }

        public void UnbindTarget()
        {
            _target = null;
        }

        private void Update()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (_target == null)
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

        public bool CanFire()
        {
            if (_target == null) return false;

            var angleToTarget = RotateCalculator.AngleTolookAt(_cabins[0].transform, _target.transform.position);
            return angleToTarget <= 10;
                
        }

    }
}
