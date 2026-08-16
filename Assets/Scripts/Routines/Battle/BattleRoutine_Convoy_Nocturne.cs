using Assets.Scripts.Enums;
using Assets.Scripts.NPC;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Routines.Battle
{
    public class BattleRoutine_Convoy_Nocturne : BattleRoutine_Convoy
    {
        protected override (EntityType enemyType, int count)[] GetEnemiesCount()
        {
            return new (EntityType enemyType, int count)[]
            {
               (EntityType.NM_Firefly , 5),
               (EntityType.NM_Pipistrelle , 1),
               (EntityType.NM_Devourer , 1),
            };
        }
    }
}
