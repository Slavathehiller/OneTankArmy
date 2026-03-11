using Assets.Scripts.DamageDealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface IMissileFactory
    {
        Missile CreateMissile(MissileType missileType);
    }
}
