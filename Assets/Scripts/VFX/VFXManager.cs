using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.VFX.Interfaces;
using UnityEngine;
using Zenject;

public class VFXManager : IVFXManager
{
    [Inject]
    private ISceneAssetFactory _sceneAssetFactory;

    public T MeakeVFXAt<T>(Vector3 position) where T : MonoBehaviour
    {
        var vfx = _sceneAssetFactory.CreateAsset<T>();
        vfx.transform.position = position;
        return vfx;
    }

    public Explosion MeakeExplosionAt(Vector3 position)
    {
        var explosion = _sceneAssetFactory.CreateAsset<Explosion>();
        explosion.transform.position = position;
        return explosion;
    }
}
