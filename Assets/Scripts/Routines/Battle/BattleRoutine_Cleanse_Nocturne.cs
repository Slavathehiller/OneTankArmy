using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scripts.Routines.Battle
{
    public class BattleRoutine_Cleanse_Nocturne : BattleRoutine_Cleanse
    {
        protected override (EntityType enemyType, int count)[] GetEnemiesCount()
        {
            return new (EntityType enemyType, int count)[]
            {
               (EntityType.NM_Firefly , 10),
               (EntityType.NM_Pipistrelle , 5),
               (EntityType.NM_Devourer , 2),
            };
        }
    }
}


