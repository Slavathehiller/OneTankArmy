using Assets.Scripts.Enums;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public abstract class BattleRoutine_BossKill_Underground : BattleRoutine_BossKill
{
    [SerializeField]
    protected AIEnemy _questEnemy;

    [Inject]
    private IPlanetManager _planetManager;
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
        SceneManager.LoadScene(_planetManager.CurrentPlanet.OutpostScene);
    }

    protected override void OnDestroyAction()
    {
        base.OnDestroyAction();
        _questEnemy.Die -= CompleteContract;
    }

}
