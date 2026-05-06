using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneNavigation
{
    public interface ISceneNavigator
    {
        public NavigationVector NavigationVector { get; set; }
        public string PortalToGo { get; set; }
        public Dictionary<string, Vector3> PortalsCoords { get; set; }

        public void ResetData();
    }
}
