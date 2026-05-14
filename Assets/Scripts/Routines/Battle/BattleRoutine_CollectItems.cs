using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Routines.Battle
{
    public class BattleRoutine_CollectItems : BattleRoutine
    {
        [SerializeField]
        private List<Transform> _questItemSpawnPoints;

        [SerializeField]
        private int _questItemsAmount = 10;

        private TankController _tankController;

        private Label _questItemsCounter;

        [Inject]
        private IQuestItemsData _questItemsData;

        protected override void ContractConditionsInit()
        {
            _document.rootVisualElement.Q<VisualElement>("CollectedItemsPanel").style.display = DisplayStyle.Flex;
            _questItemsCounter = _document.rootVisualElement.Q<Label>("ItemCollectedLabel");
            var questItemIcon = _document.rootVisualElement.Q<Image>("ItemCollectedIcon");
            questItemIcon.sprite = _questItemsData.Icon(_contractsManager.CurrentContract.QuestItemType);
            RefreshQuestItemsCounter();
        }
      
        private void RefreshQuestItemsCounter()
        {
            _questItemsCounter.text = $"{_playerSettings.GetQuestItem(_contractsManager.CurrentContract.QuestItemType)}/{_contractsManager.CurrentContract.QuestItemNeed}";
        }

        protected override void LateStart()
        {
            base.LateStart();
            for (var i = 0; i < _questItemsAmount; i++)
            {
                var spawnPointIndex = Random.Range(0, _questItemSpawnPoints.Count);
                var spawnPoint = _questItemSpawnPoints[spawnPointIndex];
                var questItem = _questItemsData.CreateQuestItem(_contractsManager.CurrentContract.QuestItemType);
                questItem.transform.position = spawnPoint.position;
                _questItemSpawnPoints.Remove(spawnPoint);
                var angle = Random.Range(0, 360);
                questItem.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        protected override void PlayerVehicleInit()
        {
            base.PlayerVehicleInit();
            _tankController = _playerVehicle.GetComponent<TankController>();
            _tankController.PickupLoot += OnPickup;
        }

        private void OnPickup(QuestItemType type, int amount)
        {
            RefreshQuestItemsCounter();
            if (_playerSettings.GetQuestItem(_contractsManager.CurrentContract.QuestItemType) >= _contractsManager.CurrentContract.QuestItemNeed)
                CompleteContract();
        }

        protected override int[] GetEnemiesCount()
        {
            return new int[4] { 6, 30, 0, 0 };
        }

        protected override void OnDestroyAction()
        {
            base.OnDestroyAction();
            _tankController.PickupLoot -= OnPickup;
        }
    }
}
