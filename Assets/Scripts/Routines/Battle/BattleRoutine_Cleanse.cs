using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Scripts.Routines.Battle
{
    public class BattleRoutine_Cleanse : BattleRoutine
    {
        private Label _targetsLeftLabel;
        protected override void ContractConditionsInit()
        {
            _targetsLeftLabel = _document.rootVisualElement.Q<Label>("TargetsLeftLabel");
            _targetsLeftLabel.style.display = DisplayStyle.Flex;
            _lifeManager.EnemyLiveCount += TargetsCountChanged;
            _lifeManager.AllEnemyDead += CompleteContract;
        }

        protected override (EntityType enemyType, int count)[] GetEnemiesCount()
        {
            return new (EntityType enemyType, int count)[]
            {
                (EntityType.AcidCockroach , 15),
                (EntityType.BoomFlea , 2),
                (EntityType.GiantScolopendra , 1),
                (EntityType.FireMantiss , 2),
            };
        }

        private void TargetsCountChanged(int targetsLeft)
        {
            _targetsLeftLabel.text = $"Осталось целей: {targetsLeft}";
        }

        protected override void OnDestroyAction()
        {
            base.OnDestroyAction();
            _lifeManager.EnemyLiveCount -= TargetsCountChanged;
            _lifeManager.AllEnemyDead -= CompleteContract;
        }
    }
}


