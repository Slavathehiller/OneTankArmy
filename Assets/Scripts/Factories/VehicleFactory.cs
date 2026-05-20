using Assets.Scripts.DamageDealers;
using Assets.Scripts.Factories.Interfaces;
using Assets.Vehicles;
using System;
using UnityEngine;
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
            var vehicle = CreateFromPath(prefabPath).GetComponent<Vehicle>();
            var minimapMarkPrefab = Resources.Load<GameObject>("Prefabs/green-dot-mark");
            var minimapMark = GameObject.Instantiate(minimapMarkPrefab, vehicle.gameObject.transform);
            minimapMark.transform.localPosition = Vector3.zero;
            return vehicle;
        }
    }
}
