using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SceneNavigation
{
    public class SceneNavigator : ISceneNavigator
    {
        public bool GoingFromAnotherScene { get ; set ; }
        public string StartPointName { get; set; }
    }
}
