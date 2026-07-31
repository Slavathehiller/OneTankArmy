using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy
{
    public class NocturneMachine : AIRangedEnemy
    {
        protected override void RangedAttack()
        {
            if (!_isDead)
            {
                Fire();
                _currentRangeAttackCooldown = _rangeAttackCooldown;
            }
        }

        protected override void InitTagCloud()
        {
            base.InitTagCloud();
            TagCloud.Add(Tag.Mechanical)
                    .Add(Tag.Heavy);
        }

        protected override void FixedUpdateActions()
        {

            if (_isDead || _target == null)
                return;

            base.FixedUpdateActions();
        }
    }
}
