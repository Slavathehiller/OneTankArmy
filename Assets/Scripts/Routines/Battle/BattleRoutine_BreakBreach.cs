using UnityEngine;
using UnityEngine.UIElements;

public abstract class BattleRoutine_BreakBreach : BattleRoutine
{
    [SerializeField]
    protected GameObject[] _breachPoints;

    [SerializeField]
    protected Transform[] _arrivePoints;

    [SerializeField]
    protected int _breacherCapacity = 20;

    [SerializeField]
    private int _breacherCrossToFail = 5;

    [SerializeField]
    protected int _initialBreacherCount = 5;

    private int _breacherCrossed = 0;

    [SerializeField]
    protected Transform _downBreachLine;

    [SerializeField]
    protected float _arrivePeriod = 3f;

    protected float _arriveTimer = 10f;


    protected Label _completeConditionLabel;
    private Label _breacherCrossedLabel;

    protected int _breacherDestroyed = 0;
    protected int _breacherLeft;

    protected abstract AIEnemy GetBreacher();

    protected abstract bool CheckIfComplete();

    protected void CreateBreacher()
    {
        var breacher = GetBreacher();
        var arrivePointIndex = Random.Range(0, _arrivePoints.Length);
        breacher.transform.position = _arrivePoints[arrivePointIndex].position;
        InitBreacher(breacher);
    }

    protected virtual void InitBreacher(AIEnemy breacher)
    {
        InitEnemy(breacher);
        breacher.Die += BreacherDestroyed;
    }

    protected override void ContractConditionsInit()
    {
        _document.rootVisualElement.Q<VisualElement>("BreacherCrossedPanel").style.display = DisplayStyle.Flex;
        _breacherCrossedLabel = _document.rootVisualElement.Q<Label>("BreacherCrossedLabel");
        _completeConditionLabel = _document.rootVisualElement.Q<Label>("CompleteConditionLabel");
        _breacherLeft = _breacherCapacity + _initialBreacherCount;
        BreacherStatsRefresh();
    }

    protected virtual void BreacherStatsRefresh()
    {
        _breacherCrossedLabel.text = $"{_breacherCrossed}/{_breacherCrossToFail}";
    }

    protected override void UpdateActions()
    {
        base.UpdateActions();
        if (_arriveTimer > 0)
            _arriveTimer -= Time.deltaTime;
        else
        {
            if (_breacherCapacity > 0)
            {
                _breacherCapacity--;
                _arriveTimer = _arrivePeriod;
                CreateBreacher();
            }
        }
    }

    protected virtual void BreacherCrossBreachLine(AIEnemy breacher)
    {
        _breacherCrossed++;
        _breacherLeft--;
        BreacherStatsRefresh();
        breacher.Die -= BreacherDestroyed;
        Destroy(breacher.gameObject);
        if (_breacherCrossed >= _breacherCrossToFail)
            FailContract();
        if (CheckIfComplete())
            CompleteContract();
    }

    protected virtual void BreacherDestroyed(BaseEntity breacher)
    {
        breacher.Die -= BreacherDestroyed;
        _breacherDestroyed++;
        _breacherLeft--;
        BreacherStatsRefresh();
        if (CheckIfComplete())
            CompleteContract();
    }
}
