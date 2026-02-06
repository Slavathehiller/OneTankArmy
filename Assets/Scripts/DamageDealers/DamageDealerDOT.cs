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
    }
}
