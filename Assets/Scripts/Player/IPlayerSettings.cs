using Assets.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Player
{
    public interface IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }
        public void LoadSettings();
        public void SaveSettings();
    }
}
