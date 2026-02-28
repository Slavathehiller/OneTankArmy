using Assets.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;
using Zenject.Asteroids;

namespace Assets.Scripts.SceneAssets
{
    public class ControlCenter : MonoBehaviour
    {
        public event UnityAction OnContractSigned;

        [SerializeField]
        private UIDocument _document;
        [SerializeField]
        private ErrorController _errorController;

        private ListView _contractsView;
        private Contracts _allContracts;
        private VisualElement _controlCenter;
        private VisualElement _controlPanel;
        private Button _signButton;
        private Label _statusLabel;

        [Inject]
        private IPlayerSettings _playerSettings;
        [Inject]
        private IContractsManager _contractManager;

        private void Start()
        {
            _controlCenter = _document.rootVisualElement.Q<VisualElement>("OperationCenterWndow");
            _contractsView = _controlCenter.Q<VisualElement>("ContractList").Q<ListView>("Contracts");
            _controlPanel = _controlCenter.Q<VisualElement>("ControlPanel");
            _allContracts = Resources.Load<Contracts>("Contracts");
            _contractsView.itemsSource = _allContracts.Data;

            _contractsView.bindItem = (element, index) =>
            {
                var contract = _allContracts.Data[index];

                element.Q<Label>("Description").text = contract.Description;
                element.SetEnabled(contract.RatingNeeded <= _playerSettings.Raiting);
            };

            _statusLabel = _controlPanel.Q<Label>("StatusLabel");

            _contractsView.selectionChanged += RefreshSelectedContract;
            _contractsView.selectedIndex = 0;

            _signButton = _controlPanel.Q<Button>("SignButton");
            _signButton.clicked += SignContract;
        }

        private void SignContract()
        {
            _contractManager.CurrentContract = (ContractData)_contractsView.selectedItem;
            _contractManager.CurrentContractStatus = ContractStatus.Signed;
            _contractManager.SaveData();
            OnContractSigned?.Invoke();
        }

        private void RefreshSelectedContract(IEnumerable<object> selectedItems)
        {
            var contract = (ContractData)_contractsView.selectedItem;
            _controlPanel.dataSource = contract;
            if (contract.RatingNeeded <= _playerSettings.Raiting) { 
                _statusLabel.text = "Доступен";}
            else
                _statusLabel.text = $"Не доступен. Требуется рейтинг {contract.RatingNeeded} и выше.";
            if (_signButton != null)
                _signButton.SetEnabled(contract.RatingNeeded <= _playerSettings.Raiting);
        }

        private void OnDestroy()
        {
            _contractsView.selectionChanged -= RefreshSelectedContract;
            _signButton.clicked -= SignContract;
        }
    }
}
