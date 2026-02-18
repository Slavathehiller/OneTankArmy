using UnityEngine;

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
                var rotateDirection = _rotateTo.Value - transform.position;
                var angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
                angle -= 90;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
        }
    }

    protected virtual bool UpdateAction() 
    {
        return true;
    }
}
