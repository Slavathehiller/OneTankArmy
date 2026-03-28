using Assets.Scripts.DamageDealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ObjectPool
{
    public interface IMissilePool
    {
        Missile GetMissile(MissileType missileType);
        void RemoveMissile(Missile missile);
        void Clear();
    }
}
