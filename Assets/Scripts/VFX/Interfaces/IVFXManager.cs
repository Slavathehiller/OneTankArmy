using UnityEngine;

namespace Assets.Scripts.VFX.Interfaces
{
    public interface IVFXManager
    {
        Explosion MeakeExplosionAt(Vector3 position);
        T MeakeVFXAt<T>(Vector3 position) where T : MonoBehaviour;
    }
}
