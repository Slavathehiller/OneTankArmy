using Assets.Scripts.DamageDealers;
using Assets.Scripts.Factories.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemy
{
    public class NM_Charon : NocturneMachine
    {
        [Inject]
        private ISceneAssetFactory _sceneAssetFactory;
        protected override void Fire()
        {
            var wirlMissile = _sceneAssetFactory.CreateAsset<WirlMissile>();
            wirlMissile.gameObject.transform.SetParent(_firePoint.transform);
            wirlMissile.transform.localPosition = Vector3.zero;
            wirlMissile.gameObject.transform.SetParent(null);

            wirlMissile.GetComponent<IncomingMissile>().SetTarget(_target);

            base.Fire();
        }
    }
}
