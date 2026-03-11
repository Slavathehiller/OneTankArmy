using Assets.Scripts.DamageDealers;
using Assets.Scripts.Factories.Interfaces;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class MissileFactory : BaseFactory, IMissileFactory
    {
        public MissileFactory(DiContainer container) : base(container)
        {
        }

        public Missile CreateMissile(MissileType missileType)
        {
            var prefabPath = PrefabsPath.GetPathForMissile(missileType);
            return CreateFromPath(prefabPath).GetComponent<Missile>();
        }
    }
}
