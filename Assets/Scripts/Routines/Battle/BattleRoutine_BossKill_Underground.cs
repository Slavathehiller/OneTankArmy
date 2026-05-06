using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BattleRoutine_BossKill_Underground : BattleRoutine_BossKill
{
    [SerializeField]
    private AIEnemy _questEnemy;

    protected override void LateStart()
    {
        base.LateStart();
        var contractCompleteLabel = _document.rootVisualElement.Q<Label>("ContractCompleteLabel");
        contractCompleteLabel.text = "Контракт выполнен. Поднимитесь на поверхность для эвакуации.";
    }

    protected override void ContractConditionsInit()
    {
        base.ContractConditionsInit();
        _questEnemy.Die += CompleteContract;
    }

    private void CompleteContract(BaseEntity arg0)
    {
        base.CompleteContract();
        RefreshQuestEnemyCounter();
    }

    protected override void OnEvacuate(BaseEntity player)
    {

    }

    protected override void OnDie(BaseEntity player)
    {
        _playerVehicle.ControlOff();
        CheckIfContractFailedOnExit();
        Invoke("ReturnToOutpost", 2);

    }

    private void ReturnToOutpost()
    {
        _sceneNavigator.ResetData();
        SceneManager.LoadScene(Scenes.OUTPOST_SCENE);
    }

    protected override void OnDestroyAction()
    {
        base.OnDestroyAction();
        _questEnemy.Die -= CompleteContract;
    }

}
