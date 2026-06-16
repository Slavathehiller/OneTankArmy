using Assets.Scripts.MISC;
using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    public class Turret : PlayerSide

    {
        [SerializeField]
        protected float _maxHP = 50;
        protected override float MaxHP => _maxHP;

        [SerializeField]
        private GameObject _mainBody;
        [SerializeField]
        private GameObject _destroyedBody;
        [SerializeField]
        protected Gun[] _guns;
        [SerializeField]
        private float _gunsRotationSpeed = 2f;
        [SerializeField]
        protected float _distanceOfDetection = 5;
        [SerializeField]
        protected float _detectionCooldown = 1;


        protected float _currentDetectionCooldown = 0;
        private AIEnemy _enemy;


        protected override void StartActions()
        {
            base.UpdateActions();
            _currentHP = MaxHP;
        }

        protected override void UpdateActions()
        {
            if (_currentDetectionCooldown > 0)
                _currentDetectionCooldown -= Time.deltaTime;

            if (_currentDetectionCooldown <= 0)
            {

                AIEnemy enemyFound = TryDetect(_distanceOfDetection);
                if (enemyFound)
                    _enemy = enemyFound;
                else
                    _enemy = null;

                _currentDetectionCooldown = _detectionCooldown;
            }

            if (_enemy != null)
            {
                var angleToEnemy = RotateCalculator.AngleTolookAt(_guns[0].transform, _enemy.transform.position);
                if (angleToEnemy <= 10)
                    FireAtEnemy();
                FollowEnemy();
            }
        }

        private void FollowEnemy()
        {
            foreach (var gun in _guns)
            {
                Vector2 direction = _enemy.transform.position - gun.transform.position;

                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                targetAngle -= 90;

                float currentAngle = gun.transform.eulerAngles.z;

                float angle = Mathf.LerpAngle(currentAngle, targetAngle, _gunsRotationSpeed * Time.deltaTime);

                gun.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void FireAtEnemy()
        {
            foreach (var gun in _guns)
                gun.TryFire();
        }

        protected AIEnemy TryDetect(float radius)
        {
            var detectedColliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in detectedColliders)
            {
                if (collider.gameObject.TryGetComponent<AIEnemy>(out var target))
                {
                    return target;
                }
            }
            return null;
        }

        protected override void CheckIfDead()
        {
            base.CheckIfDead();
            if (_isDead)
            {
                _mainBody.SetActive(false);
                _destroyedBody.SetActive(true);
                enabled = false;
            }
        }

        public override void TakeDamage(float damage)
        {
            if (_isDead) return;
            base.TakeDamage(damage);
            _currentHP -= damage;
            CheckIfDead();
        }


    }
}
