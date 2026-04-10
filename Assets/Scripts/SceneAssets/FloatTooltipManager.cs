using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneAssets
{
    public class FloatTooltipManager : IFloatTooltipManager
    {
        private FloatTooltip FloatToolTipInstance
        {
            get
            {
                var floatTooltipPrefab = Resources.Load<GameObject>("FloatTooltip");
                var floatTooltipInstance = GameObject.Instantiate(floatTooltipPrefab);
                return floatTooltipInstance.GetComponent<FloatTooltip>();
            }
        }

        public void ShowFloatTooltip(Vector3 position, string message, Sprite icon)
        {
            FloatToolTipInstance.Show(position, message, icon);
        }
    }
}
