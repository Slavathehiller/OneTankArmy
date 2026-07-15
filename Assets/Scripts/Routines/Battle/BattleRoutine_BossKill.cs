using Assets.Scripts.Enums;
using Assets.Scripts.SceneNavigation;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class BattleRoutine_BossKill : BattleRoutine
{
    protected Label _questEnemyKillCounter;
    protected override void ContractConditionsInit()
    {
        _document.rootVisualElement.Q<VisualElement>("TargetEliminationPanel").style.display = DisplayStyle.Flex;
        _questEnemyKillCounter = _document.rootVisualElement.Q<Label>("TargetEliminatedLabel");        
        RefreshQuestEnemyCounter();
    }

    protected override (EntityType enemyType, int count)[] GetEnemiesCount()
    {
        return new (EntityType enemyType, int count)[] 
        { 
            (EntityType.AcidCockroach , 30), 
            (EntityType.FireMantiss, 2) 
        };
    }

    protected void RefreshQuestEnemyCounter()
    {
        if (_contractsManager.CurrentContractStatus == ContractStatus.Completed)
        {
            _questEnemyKillCounter.text = $"{1}/{1}";
            base.CompleteContract();
        }
        else
            _questEnemyKillCounter.text = $"{0}/{1}";
    }


}
