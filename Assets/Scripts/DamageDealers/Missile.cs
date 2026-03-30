using Assets.Scripts.ObjectPool;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.DamageDealers
{

    public enum MissileType
    {
        Undefined = -1,
        AcidSpit = 0,
        Assault25mm = 1,
        Autocannon50mm = 2,
        Machinegun12mm = 3,
        Cannon100mm = 4
    }

    public abstract class Missile : DamageDealer
    {
        [SerializeField] 
        protected int _moveSpeed = 15;
        [SerializeField]
        protected float _timeOfLife;

        [SerializeField] 
        protected MissileType _missileType;

        private Rigidbody2D _rigidBody;

        [Inject]
        private IMissilePool _missilePool;
        protected Rigidbody2D RigidBody
        {
            get
            {
                if (_rigidBody == null)
                    _rigidBody = GetComponent<Rigidbody2D>();
                return _rigidBody;
            }
        }

        public MissileType MissileType => _missileType;

        public virtual void Init() 
        {
            RigidBody.AddForce(transform.up * _moveSpeed);
            if (_timeOfLife > 0)
                StartCoroutine(RemoveBulletCorutine(_timeOfLife));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //gameObject.SetActive(false);
            _missilePool.RemoveMissile(this);
        }

        private IEnumerator RemoveBulletCorutine(float latency)
        {
            yield return new WaitForSeconds(latency);
           _missilePool.RemoveMissile(this);
        }
    }
}
