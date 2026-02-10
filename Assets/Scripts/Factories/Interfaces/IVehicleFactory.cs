using Assets.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface IVehicleFactory
    {
        public Vehicle CreateVehicle(VehicleType vehicleType);
    }
}
