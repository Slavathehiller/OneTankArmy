using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField]
    private Gun[] _guns;

    [SerializeField]
    private Gun[] _auxillaryGuns;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (var gun in _guns)
            { 
                gun.TryFire();
            }
        }

        if (Input.GetMouseButton(1))
        {
            foreach (var gun in _auxillaryGuns)
            {
                gun.TryFire();
            }
        }
    }


}
