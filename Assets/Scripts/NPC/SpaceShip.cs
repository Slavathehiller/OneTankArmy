using UnityEngine;
using UnityEngine.Events;

public class SpaceShip : NPC_Base
{
    public event UnityAction HasArrived;

    [SerializeField]
    private GameObject _duseFire;

    public void MoveTo(Vector3 point)
    {
        var localScaleX = Mathf.Abs(transform.localScale.x);
        if (transform.position.x > point.x)
            transform.localScale = new Vector3(localScaleX, 1, 1);
        else
            transform.localScale = new Vector3(-localScaleX, 1, 1);

        _duseFire.SetActive(true);
        _target = point;
    }

    protected override bool UpdateAction()
    {
        if (_target == null) return false;

        if (TargetReach)
        {
            _duseFire.SetActive(false);
            _target = null;
            HasArrived?.Invoke();
            return false;
        }

        return base.UpdateAction();
    }
}
