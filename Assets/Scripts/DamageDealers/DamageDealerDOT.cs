using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DamageDealers
{
    public class DamageDealerDOT : MonoBehaviour
    {
        [SerializeField] private float _dot;

        public float DOT => _dot;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<BaseEntity>(out var entity))
            {
                entity.TakeDamage(DOT);
            }
        }

    }
}
