using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy
{
    public class NocturneMachine : AICabinEnemy
    {
        protected override void RangedAttack()
        {
            if (!_isDead)
            {
                Fire();
                _currentRangeAttackCooldown = _rangeAttackCooldown;
            }
        }

        //protected override void Fire()
        //{
        //    base.Fire();
        //    if (_missileSound != null)
        //        _missileSound.Play();
        //}

        protected override void InitTagCloud()
        {
            base.InitTagCloud();
            TagCloud.Add(Tag.Mechanical)
                    .Add(Tag.Heavy);
        }
    }
}
