using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.NPC
{
    public class NPC_ConvoyMember : NPC_Civilian
    {
        public event UnityAction<NPC_ConvoyMember> EscapePointReach;
        [SerializeField]
        private Transform _escapePoint;

        private bool _escaped;
        public bool Escaped => _escaped;

        protected override void UpdateActions()
        {
            base.UpdateActions();
            if (Vector3.Distance(transform.position, _escapePoint.position) <= 0.1f)
            {
                _escaped = true;
                gameObject.SetActive(false);
                EscapePointReach?.Invoke(this);                  
            }
        }

    }
}
