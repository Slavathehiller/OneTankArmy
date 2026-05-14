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

        protected override int[] GetEnemiesCount()
        {
            //return new int[4] { 1, 0, 0, 0 };
            return new int[4] { 15, 2, 1, 2 }; 
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


