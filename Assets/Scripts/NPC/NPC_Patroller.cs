using UnityEngine;

[System.Serializable]
public struct Waypoint
{
    public Transform point;
    public float latency;
}
public abstract class NPC_Patroller : NPC_Base
{
    [SerializeField]
    protected Waypoint[] _waypoints;

    [SerializeField]
    protected bool _cycleMoving = true;

    protected int _currentWaypointIndex = 0;

    protected float _latency;

    private void Start()
    {
        _target = _waypoints[0].point.position;
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

        _currentWaypointIndex++;
        if (_currentWaypointIndex >= _waypoints.Length)
            if (_cycleMoving)
                _currentWaypointIndex = 0;
            else
            {
                _target = null;
                return false;
            }

        _target = _waypoints[_currentWaypointIndex].point.position;
        _latency = _waypoints[_currentWaypointIndex].latency;
        return false;
    }

    
}
