using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField] 
    private GameObject _followObject;
    [SerializeField]
    private Vector3 _offset = new Vector3(0f, 0, -10f);

    [Header("Smoothing")]
    [SerializeField]
    private bool _smoothing;
    [SerializeField]
    private float _smoothTime = 0.15f;

    private Vector3 _velocity;

    public void BindObject(GameObject followObject)
    {
        _followObject = followObject;
    }

    void Update()
    {
        if (_followObject == null)
            return;
        var x = _followObject.transform.position.x;
        var y = _followObject.transform.position.y;
        transform.position = new Vector3(x, y, -10);
    }

    //private void LateUpdate()
    //{
    //    if (_followObject == null)
    //        return;
    //    var desiredPosition = _followObject.transform.position + _offset;
    //    if (!_smoothing)
    //    {
    //        transform.position = desiredPosition;
    //        _velocity = Vector3.zero;
    //        return;
    //    }

    //    transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothTime);
    //}

}
