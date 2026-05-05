using Assets.Scripts.Player;
using Assets.Vehicles;
using System;
using System.Collections.Generic;

namespace Assets.Player
{
    public interface IPlayerSettings
    {
        public VehicleType CurrentVehicle { get; set; }
        public float CurrentHealth { get; set; }
        public DateTime? RepairEndTime { get; set; }
        public int Rating { get; set; }
        public int Money { get; set; }
        public int CurrentPlanetID { get; set; }

        public int GetConsumable(Consumables value);
        public void AddConsumable(Consumables value);
        public void RemoveConsumable(Consumables value);

        public int GetQuestItem(QuestItemType value);
        public void AddQuestItems(QuestItemType questItemType, int amount);
        public void RemoveQuestItems(QuestItemType questItemType, int amount);

        public void LoadSettings();
        public void SaveSettings();
    }
}
