using UnityEngine;

namespace Assets.Scripts.VFX.Interfaces
{
    public interface IVFXManager
    {
        Explosion MakeExplosionAt(Vector3 position, float scale = 1);
        T MakeVFXAt<T>(Vector3 position) where T : MonoBehaviour;
    }
}
