using Assets.Scripts.Enums;
using Assets.Scripts.Factories.Interfaces;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class EntityFactory : BaseFactory, IEntityFactory
    {
        public EntityFactory(DiContainer container) : base(container)
        {
        }

        public BaseEntity CreateEntity(EntityType entityType)
        {
            var prefabPath = PrefabsPath.GetPathForEntity(entityType);
            var entity = CreateFromPath(prefabPath).GetComponent<BaseEntity>();

            return entity;
        }
    }
}
