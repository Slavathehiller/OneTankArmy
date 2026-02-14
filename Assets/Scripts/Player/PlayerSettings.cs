using Assets.Player;
using Assets.Vehicles;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSettings : IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }

        private const string CURRENT_VEHICLE = "CurrentVehicle";

        public void ChangeVechicle(VehicleType vehicle)
        {
            CurrentVehicle = vehicle;
            SaveSettings();
        }

        public PlayerSettings()
        {
            LoadSettings();
        }
        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(CURRENT_VEHICLE))
                CurrentVehicle = (VehicleType)PlayerPrefs.GetInt(CURRENT_VEHICLE);
            else
                CurrentVehicle = VehicleType.Beetle;
        }

        public void SaveSettings() 
        {
            PlayerPrefs.SetInt(CURRENT_VEHICLE, (int)CurrentVehicle);
        }
    }
}
