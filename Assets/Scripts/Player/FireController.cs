using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField]
    private Gun[] _guns;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (var gun in _guns)
            { 
                gun.TryFire();
            }
        }
    }


}
