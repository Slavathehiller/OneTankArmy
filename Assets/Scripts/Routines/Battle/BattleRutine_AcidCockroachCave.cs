using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Routines.Battle
{
    public class BattleRutine_AcidCockroachCave : BattleRoutine_BossKill_Underground
    {
        private AcidCockroachQueen _queen;
        protected override void LateStart()
        {
            base.LateStart();
            if (_questEnemy != null)
            {
                _queen = _questEnemy as AcidCockroachQueen;
                if (_queen == null)
                {
                    Debug.LogError("Quest enemy not an Acid cocroach queen.");
                    return;
                }
                _queen.Spawn += AcidCockroachSpawn;
            }
        }

        private void AcidCockroachSpawn()
        {
            if (_lifeManager.EnemyLiveNow() > 50)
                return;
            var cockroach = _sceneAssetFactory.CreateAsset<AcidCockroach>();
            _lifeManager.AddEnemy(cockroach);
            cockroach.transform.position = _queen.SpawnPoint.position;
            cockroach.transform.rotation = _queen.SpawnPoint.rotation;
            cockroach.GetShocked();
            cockroach.ForcedMove(cockroach.transform.up * 40);
            cockroach.SetAgentOn();
        }

        protected override void OnDestroyAction()
        {
            base.OnDestroyAction();
            if (_queen != null)
            _queen.Spawn -= AcidCockroachSpawn;
        }
    }
}
