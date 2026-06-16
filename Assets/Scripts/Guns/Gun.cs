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
    [SerializeField] 
    protected int _fireSeries = 1;
    [SerializeField] 
    protected float _fireSeriesLatency = 0.1f;

    private float _fireCooldown;
    private float _fireSeriesCooldown;
    private int _fireSeriesCount;

    public void TryFire()
    {
        if (_fireCooldown <= 0)
        {
            _fireSeriesCount = _fireSeries;
            _fireCooldown = _fireLatency;
        }
    }

    private void Update()
    {
        if (_fireSeriesCooldown <= 0 && _fireSeriesCount > 0)
        {
            Fire();
            _fireSeriesCount--;
            _fireSeriesCooldown = _fireSeriesLatency;
        }

        if (_fireCooldown > 0)
            _fireCooldown -= Time.deltaTime;
        if (_fireSeriesCooldown > 0)
            _fireSeriesCooldown -= Time.deltaTime;
    }

    protected abstract void Fire();

}
