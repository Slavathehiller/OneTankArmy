using Assets.Scripts.DamageDealers;
using Assets.Scripts.Enums;
using Assets.Vehicles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public static class PrefabsPath
    {

        private static Dictionary<Type, string> _pathesByClass = new();
        private static Dictionary<MissileType, string> _pathesByMissileType = new();
        private static Dictionary<VehicleType, string> _pathesByVehicleType = new();
        private static Dictionary<EntityType, string> _pathesByEntityType = new();

        public static void Register(Type type, string path)
        {
            if (_pathesByClass.ContainsKey(type))
            {
                Debug.LogError($"Type {type.FullName} already registered in PrefabPath.");
                return;
            }

            _pathesByClass.Add(type, path);
        }

        public static void Register(MissileType missileType, string path)
        {
            if (_pathesByMissileType.ContainsKey(missileType))
            {
                Debug.LogError($"MissileType {missileType} already registered in PrefabPath.");
                return;
            }

            _pathesByMissileType.Add(missileType, path);
        }

        public static void Register(VehicleType vehicleType, string path)
        {
            if (_pathesByVehicleType.ContainsKey(vehicleType))
            {
                Debug.LogError($"VehicleType {vehicleType} already registered in PrefabPath.");
                return;
            }

            _pathesByVehicleType.Add(vehicleType, path);
        }

        public static void Register(EntityType entityType, string path)
        {
            if (_pathesByEntityType.ContainsKey(entityType))
            {
                Debug.LogError($"VehicleType {entityType} already registered in PrefabPath.");
                return;
            }

            _pathesByEntityType.Add(entityType, path);
        }


        public static string GetPathFor<T>()
        {
            string result;
            if (!_pathesByClass.TryGetValue(typeof(T), out result))
            {
                Debug.LogError($"Type {typeof(T).FullName} not registered in PrefabPath.");
            }
            return result;
        }

        public static string GetPathForMissile(MissileType missileType)
        {
            string result;
            if (!_pathesByMissileType.TryGetValue(missileType, out result))
            {
                Debug.LogError($"MissileType {missileType} not registered in PrefabPath.");
            }
            return result;
        }

        public static string GetPathForVehicle(VehicleType VehicleType)
        {
            string result;
            if (!_pathesByVehicleType.TryGetValue(VehicleType, out result))
            {
                Debug.LogError($"VehicleType {VehicleType} not registered in PrefabPath.");
            }
            return result;
        }

        public static string GetPathForEntity(EntityType entityType)
        {
            string result;
            if (!_pathesByEntityType.TryGetValue(entityType, out result))
            {
                Debug.LogError($"EntityType {entityType} not registered in PrefabPath.");
            }
            return result;
        }

        public static void InitPathes()
        {
            Register(typeof(Explosion), "Prefabs/Explosion");
            Register(typeof(AcidCockroach), "Prefabs/AcidCockroach");
            Register(typeof(BoomFlea), "Prefabs/BoomFlea");
            Register(typeof(GiantScolopendra), "Prefabs/GiantScolopendra");
            Register(typeof(FireMantiss), "Prefabs/FireMantiss");
            Register(typeof(FireMantiss_Breacher), "Prefabs/FireMantiss_Breacher");           
            Register(typeof(AcidGoo), "Prefabs/AcidGoo");
            Register(typeof(BigAcidGoo), "Prefabs/BigAcidGoo");
            Register(typeof(ToxicGoo), "Prefabs/ToxicGoo");
            Register(typeof(BigGreenGoo), "Prefabs/BigGreenGoo");            
            Register(typeof(Flame_old), "Prefabs/Flame");


            Register(EntityType.AcidCockroach, "Prefabs/AcidCockroach");
            Register(EntityType.BoomFlea, "Prefabs/BoomFlea");
            Register(EntityType.GiantScolopendra, "Prefabs/GiantScolopendra");
            Register(EntityType.FireMantiss, "Prefabs/FireMantiss");
            Register(EntityType.FireMantiss_Breacher, "Prefabs/FireMantiss_Breacher");           
            Register(EntityType.NM_Firefly, "Prefabs/NM_Firefly");


            Register(VehicleType.Beetle, "Prefabs/Vehicle/Beetle");
            Register(VehicleType.DianBao, "Prefabs/Vehicle/DianBao");
            Register(VehicleType.Fury, "Prefabs/Vehicle/Fury");
            Register(VehicleType.Ratnik, "Prefabs/Vehicle/Ratnik");
            Register(VehicleType.Castellan, "Prefabs/Vehicle/Castellan");

            Register(MissileType.AcidSpit, "Prefabs/AcidSpit");
            Register(MissileType.Assault25mm, "Prefabs/Bullets/AssaultCannon25mmBullet");
            Register(MissileType.Autocannon50mm, "Prefabs/Bullets/Autocannon50mmBullet");
            Register(MissileType.Machinegun12mm, "Prefabs/Bullets/Machinegun12mmBullet");
            Register(MissileType.Cannon100mm, "Prefabs/Bullets/Cannon100mmBullet");
            Register(MissileType.PhotonCharge, "Prefabs/Bullets/PhotonCharge");
        }
    }
}
