using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _followObject;

    public void BindObject(GameObject followObject)
    {
        _followObject = followObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_followObject == null)
            return;
        var x = _followObject.transform.position.x;
        var y = _followObject.transform.position.y;
        transform.position = new Vector3(x, y, -10);
    }
}
