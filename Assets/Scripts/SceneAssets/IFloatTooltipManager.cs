using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneAssets
{
    public interface IFloatTooltipManager
    {
        void ShowFloatTooltip(Vector3 position, string message, Sprite icon);
    }
}
