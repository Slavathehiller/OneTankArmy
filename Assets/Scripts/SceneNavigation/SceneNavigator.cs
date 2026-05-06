using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneNavigation
{

    public enum NavigationVector
    {
        Undefined = -1,
        StartGame = 0,
        GoingToMission = 1,
        ReturnFromMission = 2,
        GoToNextLevel = 3,
        GoToPreviousLevel = 4,
        GoToOrbit = 5,
        ReturnFromOrbit = 6
    }

    public struct NavigationPoint
    {
        public string SceneName;
        public string PortalName;
    }

    public class SceneNavigator : ISceneNavigator
    {
        public NavigationVector NavigationVector { get ; set ; }
        public string PortalToGo { get; set; }
        public Dictionary<string, Vector3> PortalsCoords { get; set; } = new();

        public void ResetData()
        {
            NavigationVector = NavigationVector.ReturnFromMission;
            PortalToGo = "";
            PortalsCoords.Clear();
        }
    }
}
