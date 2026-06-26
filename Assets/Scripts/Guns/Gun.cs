using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] 
    protected GameObject[] _firePoints;
    [SerializeField] 
    protected AudioSource _audioSourceFire;
    [SerializeField] 
    protected AudioClip _fireSound;
    [SerializeField] 
    protected float _fireLatency;
    protected float _fireCooldown;

    private void Update()
    {
        UpdateActions();
    }

    public virtual void TryFire()
    {
        if (_fireCooldown <= 0)
        {
            Fire();
            _fireCooldown = _fireLatency;
        }
    }

    protected virtual void UpdateActions()
    {
        if (_fireCooldown > 0)
            _fireCooldown -= Time.deltaTime;

    }
    protected abstract void Fire();

}
