using UnityEngine;

public abstract class NPC_Base : MonoBehaviour
{
    [SerializeField]
    protected float _moveSpeed;

    protected Vector3? _target;

    protected virtual bool TargetReach
    {
        get
        {
            return Vector3.Distance(transform.position, _target.Value) <= 0.01f;
        }
    }

    private void Update()
    {
        if (!UpdateAction())
            return;
        if (_target != null && !TargetReach)
        {
            Vector3 direction = _target.Value - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
        }
    }

    protected virtual bool UpdateAction() 
    {
        return true;
    }
}
