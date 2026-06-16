using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleRoutine_BreakBreach_MantissMigration : BattleRoutine_BreakBreach
{

    private Label _breacherDestroyedLabel;

    protected override void ContractConditionsInit()
    {
        _breacherDestroyedLabel = _document.rootVisualElement.Q<Label>("BreacherDestroyedLabel");
        base.ContractConditionsInit();
        _completeConditionLabel.text = "Противников:";
    }

    protected override int[] GetEnemiesCount()
    {
        return new int[4] {3, 1, 0, 0};
    }

    public override void SpawnEnemies()
    {
        base.SpawnEnemies();
        for (var i = 0; i < _initialBreacherCount; i++)
        {
            var mantiss = SpawnEnemy<FireMantiss_Breacher>();
            InitMantiss(mantiss);
        }
    }

    protected override AIEnemy GetBreacher()
    {
        return _sceneAssetFactory.CreateAsset<FireMantiss_Breacher>();
    }

    private void InitMantiss(FireMantiss_Breacher mantiss)
    {
        mantiss.BindBreachPoints(_breachPoints);
        mantiss.BindBreachLine(_downBreachLine);
        mantiss.CrossBreachLine += BreacherCrossBreachLine;
        mantiss.Die += BreacherDestroyed;
    }

    protected override void InitBreacher(AIEnemy breacher)
    {
        base.InitEnemy(breacher);
        InitMantiss((FireMantiss_Breacher)breacher);
    }

    protected override bool CheckIfComplete()
    {
        return _breacherLeft <= 0;
    }

    protected override void BreacherStatsRefresh()
    {
        base.BreacherStatsRefresh();
        _breacherDestroyedLabel.text = $"{_breacherLeft}";
    }

}
