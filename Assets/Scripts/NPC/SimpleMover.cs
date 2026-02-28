using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.NPC
{
    public class SimpleMover : NPC_Base
    {
        private UnityAction _onTargetReachMethod;
        public void MoveTo(Vector3 point, float moveSpeed = 1, UnityAction callback = null)
        {
            RotateTo(point);
            _moveSpeed = moveSpeed;
            _target = point;
            _onTargetReachMethod = callback;
        }

        protected override bool UpdateAction()
        {
            if (TargetReach)
            {
                _target = null;
                _onTargetReachMethod?.Invoke();
            }
            return base.UpdateAction();
        }
    }
}
