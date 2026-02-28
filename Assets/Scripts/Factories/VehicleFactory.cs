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
            switch (vehicleType)
            {
                case VehicleType.Beetle: 
                    return Create<Beetle>();
                case VehicleType.DianBao:
                    return Create<DianBao>();
                case VehicleType.Fury:
                    return Create<Fury>();
                default:
                    throw new ArgumentException($"There is no vehicle class for type {vehicleType}");
            }                            
        }
    }
}
