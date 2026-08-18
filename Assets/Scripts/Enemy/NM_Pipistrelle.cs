using Assets.Scripts.DamageDealers;
using Assets.Scripts.Enemy;
using Assets.Scripts.Factories.Interfaces;
using Assets.Scripts.ObjectPool;
using System.Drawing;
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

    protected override void FixedUpdateActions()
    {
        if (_isDead || _target == null)
            return;
        base.FixedUpdateActions();
        if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfRangeAttack)
        {
            StopMoving();
            if (_currentRangeAttackCooldown <= 0 && !TryToRotateAt(_target.transform.position))
                RangedAttack();
        }
    }
}
