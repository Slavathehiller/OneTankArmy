using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Shuttle : NPC_Base
{
    protected override bool TargetReach
    {
        get
        {
            return Vector3.Distance(transform.position, _target.Value) <= 0.05f;
        }
    }

    public void MoveToPoint(Vector3 point, UnityAction callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPointCoroutine(point, callback));
    }

    public void TakeOff(UnityAction callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(TakeOffCoroutine(callback));
    }


    private IEnumerator MoveToPointCoroutine(Vector3 point, UnityAction callback)
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(true);
        var startPosition = transform.position;
        var startScale = transform.localScale.x;
        var endScale = 5;
        var duration = 2f;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(startPosition, point, t);
            var currentScale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = Vector3.one * currentScale;

            yield return null;
        }

        transform.position = point;
        transform.localScale = Vector3.one * endScale;
        callback?.Invoke();
    }

    private IEnumerator TakeOffCoroutine(UnityAction callback = null)
    {
        var startScale = transform.localScale.x;
        var maxScale = 15;

        yield return new WaitForSeconds(1);
        transform.localScale = Vector3.one * startScale;

        var elapsed = 0f;
        var growDuration = 2f;
        var startPosition = transform.position;

        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            transform.position += transform.up * _moveSpeed * Time.deltaTime;
            float currentScale = Mathf.Lerp(startScale, maxScale, elapsed / growDuration);
            transform.localScale = Vector3.one * currentScale;

            yield return null;
        }

        gameObject.SetActive(false); 
    }


}
