using Assets.Scripts.DamageDealers;
using Assets.Scripts.Enemy;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.ObjectPool;
using UnityEngine;
using Zenject;

public class NM_Pipistrelle : NocturneMachine
{
    [Inject]
    private ISceneAssetFactory _sceneAssetFactory;
    protected override void Fire()
    {
        var bolt = _sceneAssetFactory.CreateAsset<SmallGravityBolt>();
        bolt.transform.position = _firePoint.transform.position;
        bolt.transform.rotation = _firePoint.transform.rotation;

        bolt.Init();
        if (_missileSound != null)
            _missileSound.Play();
    }
}
