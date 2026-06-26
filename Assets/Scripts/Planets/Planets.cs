using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Planets
{
    [System.Serializable]
    public class PlanetInfo
    {
        public int ID;
        public Sprite Image;        
        public string Name;
        public string Description;
        public string OutpostScene;
    }

    [CreateAssetMenu(fileName = "Planets", menuName = "Scriptable Objects/Planets")]
    public class Planets : ScriptableObject
    {
        public List<PlanetInfo> Data;
    }
}
