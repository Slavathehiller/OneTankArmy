using Assets.Vehicles;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class VehiclePresenter
{
    public VehicleType VehicleType;
    public string Name;
    public Sprite Portrait;
    public string Vitality;
    public string Speed;
    public string Firepower;
}

[CreateAssetMenu(fileName = "VehiclePresenters", menuName = "Scriptable Objects/VehiclePresenters")]
public class VehiclePresenters : ScriptableObject
{
    public List<VehiclePresenter> Data;
}
