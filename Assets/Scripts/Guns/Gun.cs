using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GameObject[] _firePoints;
    [SerializeField] protected AudioSource _audioSourceFire;
    [SerializeField] protected AudioClip _fireSound;
    [SerializeField] protected float _fireLatency;
    private float _fireCooldown;


    public void TryFire()
    {
        if (_fireCooldown <= 0)
        {
            Fire();
            _fireCooldown = _fireLatency;
        }
    }

    private void Update()
    {
        if (_fireCooldown > 0)
            _fireCooldown -= Time.deltaTime;
    }
    protected abstract void Fire();

}
