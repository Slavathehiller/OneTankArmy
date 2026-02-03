using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected float _rotateSpeed = 100;

    private Rigidbody2D _rigidBody;

    protected Rigidbody2D RigidBody
    {
        get
        {
            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody2D>();
            return _rigidBody;
        }
    }

    protected abstract void CheckIfDead();

}
