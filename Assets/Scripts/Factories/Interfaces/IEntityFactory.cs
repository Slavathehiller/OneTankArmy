using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface IEntityFactory
    {
        BaseEntity CreateEntity(EntityType entityType);
    }
}
