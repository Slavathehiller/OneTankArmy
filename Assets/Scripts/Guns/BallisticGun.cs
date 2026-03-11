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
}
