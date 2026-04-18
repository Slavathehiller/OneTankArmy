using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.NPC
{
    public class Transporter : NPC_Base
    {
        public void TakeOff(UnityAction callback = null)
        {
            StopAllCoroutines();
            StartCoroutine(TakeOffCoroutine(callback));
        }

        private IEnumerator TakeOffCoroutine(UnityAction callback = null)
        {
            var startScale = transform.localScale.x;
            var maxScale = 10;

            transform.localScale = Vector3.one * startScale;

            var elapsed = 0f;
            var growDuration = 2f;

            while (elapsed < growDuration)
            {
                elapsed += Time.deltaTime;
                float currentScale = Mathf.Lerp(startScale, maxScale, elapsed / growDuration);
                transform.localScale = Vector3.one * currentScale;

                yield return null;
            }

            callback?.Invoke();
        }
    }
}
