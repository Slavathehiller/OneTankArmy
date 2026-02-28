using UnityEngine;
using UnityEngine.Events;

public abstract class NPC_Base : MonoBehaviour
{
    [SerializeField]
    protected float _moveSpeed = 1;

    protected Vector3? _target;

    protected Vector3? _rotateTo;

    protected virtual bool TargetReach
    {
        get
        {
            return _target != null && Vector3.Distance(transform.position, _target.Value) <= 0.01f;
        }
    }

    private void Update()
    {
        if (!UpdateAction())
            return;
        if (_target != null && !TargetReach)
        {
            Vector3 direction = _target.Value - transform.position;
            if (_rotateTo != null)
            {
                RotateTo(_rotateTo.Value);
            }
            transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
        }
    }

    public virtual void RotateTo(Vector3 point, UnityAction callback = null)
    {
        var rotateDirection = point - transform.position;
        var angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
        angle -= 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        callback?.Invoke();
    }

    protected virtual bool UpdateAction() 
    {
        return true;
    }
}
