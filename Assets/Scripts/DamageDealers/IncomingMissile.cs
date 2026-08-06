using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.DamageDealers
{
    public class IncomingMissile : MonoBehaviour
    {
        [SerializeField]
        protected float _moveSpeed;

        [SerializeField]
        protected float _lifeTime;

        private float _currentLifeTime;

        private GameObject _target;

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        private void Update()
        {
            _currentLifeTime += Time.deltaTime;
            if (_currentLifeTime >= _lifeTime)
                Destroy(gameObject);

            var moveDirection = (_target.transform.position - transform.position).normalized;

            transform.position += (Vector3)moveDirection * _moveSpeed * Time.deltaTime;
        }
    }
}
