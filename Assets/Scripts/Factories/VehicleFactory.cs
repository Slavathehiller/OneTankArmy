using Assets.Scripts.DamageDealers;
using Assets.Scripts.Factories.Interfaces;
using Assets.Vehicles;
using System;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class VehicleFactory : BaseFactory, IVehicleFactory
    {
        public VehicleFactory(DiContainer container) : base(container)
        {
        }

        public Vehicle CreateVehicle(VehicleType vehicleType)
        {
            var prefabPath = PrefabsPath.GetPathForVehicle(vehicleType);
            return CreateFromPath(prefabPath).GetComponent<Vehicle>();
        }
    }
}
