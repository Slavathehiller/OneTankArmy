using Assets.Scripts.SceneAssets;
using UnityEngine;
using Zenject;

public class QuestItem : MonoBehaviour
{
    [SerializeField]
    private QuestItemType _questItemType;

    [SerializeField]
    private int _minAmount;

    [SerializeField]
    private int _maxAmount;

    [Inject]
    private IFloatTooltipManager _floatTooltipManager;

    [Inject]
    private IQuestItemsData _questItemsData;

    public (QuestItemType type, int amount) GetLoot()
    {
        var amount = Random.Range(_minAmount, _maxAmount + 1);
        _floatTooltipManager.ShowFloatTooltip(transform.position, $"+ {amount}", _questItemsData.Icon(_questItemType));
        return (_questItemType, amount);
    }

}
