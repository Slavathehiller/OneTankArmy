using Assets.Scripts.DamageDealers;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissilePool : IMissilePool
{
    private Dictionary<MissileType, List<Missile>> _pool = new();

    [Inject]
    private IMissileFactory _missileFactory;
    private List<Missile> GetBulletList(MissileType type)
    {
        if (!_pool.TryGetValue(type, out var bulletList))
        {
            bulletList = new List<Missile>();
            _pool.Add(type, bulletList);
        }
        return bulletList;
    }

    public Missile GetMissile(MissileType missileType)
    {
        Missile result;
        var bulletList = GetBulletList(missileType);

        if (bulletList.Count > 0)
        {
            result = bulletList[bulletList.Count - 1];
            result.gameObject.SetActive(true);
            bulletList.Remove(result);
        }
        else
        {
            result = _missileFactory.CreateMissile(missileType);
        }
        return result;
    }

    public void RemoveMissile(Missile missile)
    {
        var bulletList = GetBulletList(missile.MissileType);
        bulletList.Add(missile);
        missile.gameObject.SetActive(false);
    }
}
