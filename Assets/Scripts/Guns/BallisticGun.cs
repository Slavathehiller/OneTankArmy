using Assets.Scripts.DamageDealers;
using Assets.Scripts.ObjectPool;
using System.Collections;
using UnityEngine;
using Zenject;

public class BallisticGun : Gun
{
    [SerializeField]
    protected MissileType _missileType;

    [Inject]
    private IMissilePool _missilePool;

    [SerializeField]
    protected int _fireSeries = 1;
    [SerializeField]
    protected float _fireSeriesLatency = 0.1f;
    private float _fireSeriesCooldown;
    private int _fireSeriesCount;


    protected override void Fire()
    {
        foreach (var firePoint in _firePoints)
        {
            var bullet = _missilePool.GetMissile(_missileType);
            bullet.transform.position = firePoint.transform.position;
            bullet.transform.rotation = firePoint.transform.rotation;
            _audioSourceFire.PlayOneShot(_fireSound);
            bullet.Init();
        }
    }

    public override void TryFire()
    {
        if (_fireCooldown <= 0)
        {
            _fireSeriesCount = _fireSeries;
            _fireCooldown = _fireLatency;
        }
    }

    protected override void UpdateActions()
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


    private void OnDestroy()
    {
        _missilePool.Clear();
    }
}
