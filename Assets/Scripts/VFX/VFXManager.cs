using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.VFX.Interfaces;
using UnityEngine;
using Zenject;

public class VFXManager : IVFXManager
{
    [Inject]
    private ISceneAssetFactory _sceneAssetFactory;

    public T MakeVFXAt<T>(Vector3 position) where T : MonoBehaviour
    {
        var vfx = _sceneAssetFactory.CreateAsset<T>();
        vfx.transform.position = position;
        return vfx;
    }

    public Explosion MakeExplosionAt(Vector3 position, float scale = 1)
    {
        var explosion = _sceneAssetFactory.CreateAsset<Explosion>();
        explosion.transform.position = position;
        explosion.transform.localScale = new Vector3(scale, scale, 1);
        explosion.ScaleForce(scale);
        return explosion;
    }
}
