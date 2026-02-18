using UnityEngine;


public enum PatrolingStrategy
{
    Cycle,
    OneWay,
    TwoWay
}

[System.Serializable]
public struct Waypoint
{
    public Transform pointToMove;
    public bool rotateToAnotherPoint;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public Transform? pointToRotate;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public float latency;
}
public abstract class NPC_Patroller : NPC_Base
{
    [SerializeField]
    protected Waypoint[] _waypoints;

    [SerializeField]
    public PatrolingStrategy _strategy;

    protected int _currentWaypointIndex = 0;

    protected bool _reverseWay;

    protected float _latency;

    private void Start()
    {
        _target = _waypoints[0].pointToMove.position;
        if (_waypoints[0].pointToRotate != null)
            _rotateTo = _waypoints[0].pointToRotate.position;
        _reverseWay = false;
    }

    protected override bool UpdateAction()
    {
        if (_waypoints.Length == 0)
            return false;

        if (_latency > 0)
        {
            _latency -= Time.deltaTime;
            return false;
        }

        if (!TargetReach)
            return true;

        if (_reverseWay)
            _currentWaypointIndex--;
        else
            _currentWaypointIndex++;
        if (_currentWaypointIndex >= _waypoints.Length || _currentWaypointIndex < 0)
            switch (_strategy)
            {
                case PatrolingStrategy.Cycle:
                    _currentWaypointIndex = 0;
                    break;
                case PatrolingStrategy.OneWay:
                    _target = null;
                    return false;
                case PatrolingStrategy.TwoWay:
                    _reverseWay = !_reverseWay;
                    return false;

            }

        _target = _waypoints[_currentWaypointIndex].pointToMove.position;

        _latency = _waypoints[_currentWaypointIndex].latency;
        if (_waypoints[_currentWaypointIndex].rotateToAnotherPoint)
            _rotateTo = _waypoints[_currentWaypointIndex].pointToRotate != null ? _waypoints[_currentWaypointIndex].pointToRotate.position : null;
        else
            _rotateTo = _target;
        return false;
    }   
}
