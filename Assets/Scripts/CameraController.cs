using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _followObject;

    // Update is called once per frame
    void Update()
    {
        var x = _followObject.transform.position.x;
        var y = _followObject.transform.position.y;
        transform.position = new Vector3(x, y, -10);
    }
}
