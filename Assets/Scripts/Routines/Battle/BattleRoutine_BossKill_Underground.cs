using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BattleRoutine_BossKill_Underground : BattleRoutine_BossKill
{
    [SerializeField]
    protected AIEnemy _questEnemy;

    protected override void LateStart()
    {
        var contractCompleteLabel = _document.rootVisualElement.Q<Label>("ContractCompleteLabel");
        contractCompleteLabel.text = "Контракт выполнен. Поднимитесь на поверхность для эвакуации.";
        if (_contractsManager.CurrentContractStatus == ContractStatus.Completed)
        {
            Destroy(_questEnemy.gameObject);
            CompleteContract(null);
        }
        base.LateStart();
    }

    protected override int[] GetEnemiesCount()
    {
        return new int[4] { 40, 0, 0, 1 };
    }

    protected override void ContractConditionsInit()
    {
        base.ContractConditionsInit();
        if (_contractsManager.CurrentContractStatus != ContractStatus.Completed)
            _questEnemy.Die += CompleteContract;
    }

    private void CompleteContract(BaseEntity entity)
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
