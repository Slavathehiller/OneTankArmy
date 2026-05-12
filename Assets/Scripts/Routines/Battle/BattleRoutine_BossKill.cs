using Assets.Scripts.SceneNavigation;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class BattleRoutine_BossKill : BattleRoutine
{
    protected Label _questEnemyKillCounter;
    protected override void ContractConditionsInit()
    {
        _questEnemyKillCounter = _document.rootVisualElement.Q<Label>("TargetEliminatedLabel");
        RefreshQuestEnemyCounter();
    }

    protected override int[] GetEnemiesCount()
    {
        return new int[4] { 30, 0, 0, 2 };
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
