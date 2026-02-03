using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DamageDealers
{
    public abstract class Missile : DamageDealer
    {
        [SerializeField] 
        protected int _moveSpeed = 15;
        [SerializeField]
        private float _timeOfLife;

        private Rigidbody2D _rigidBody;

        protected Rigidbody2D RigidBody
        {
            get
            {
                if (_rigidBody == null)
                    _rigidBody = GetComponent<Rigidbody2D>();
                return _rigidBody;
            }
        }

        void Start()
        {
            Init();            
        }

        protected virtual void Init() 
        {
            RigidBody.AddForce(transform.up * _moveSpeed);
            if (_timeOfLife > 0) 
                Destroy(gameObject, _timeOfLife);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            gameObject.SetActive(false);
        }
    }
}
