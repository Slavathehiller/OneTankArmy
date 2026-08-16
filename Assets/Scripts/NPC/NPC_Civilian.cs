using Assets.Scripts.Player;
using Assets.Scripts.VFX.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPC
{
    public abstract class NPC_Civilian : PlayerSide
    {
        [Inject]
        private IVFXManager _VFXMmanager;

        [SerializeField]
        private float _maxHP;
        protected override float MaxHP => _maxHP;

        private NPC_Patroller _moveController;
        protected override void StartActions()
        {
            base.StartActions();
            _currentHP = MaxHP;
            _moveController = GetComponent<NPC_Patroller>();
            var minimapMarkPrefab = Resources.Load<GameObject>("Prefabs/yellow-dot-mark");
            var minimapMark = GameObject.Instantiate(minimapMarkPrefab);
            minimapMark.transform.SetParent(_mainBody.transform);
            minimapMark.transform.localPosition = Vector3.zero;
        }

        protected override void InitTagCloud()
        {
            base.InitTagCloud();
            TagCloud.Add(Tag.Mechanical)
                    .Add(Tag.Heavy);
        }

        public override void TakeDamage(float damage)
        {
            if (_isDead) return;
            base.TakeDamage(damage);
            _currentHP -= damage;
            CheckIfDead();
        }

        protected override void DeadPerfomance()
        {
            base.DeadPerfomance();
            _moveController.enabled = false;
            _VFXMmanager.MakeExplosionAt(transform.position, 3);
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<DamageDealer>(out var dd))
            {
                TakeDamage(dd.Damage);
                ReactToDamage(dd);
                dd.ReactToCollision(gameObject);
            }
        }

    }
}
