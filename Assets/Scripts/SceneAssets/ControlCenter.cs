using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

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
        [Inject]
        private IPlanetManager _planetManager;

        private void Start()
        {
            _controlCenter = _document.rootVisualElement.Q<VisualElement>("OperationCenterWndow");
            _contractsView = _controlCenter.Q<VisualElement>("ContractList").Q<ListView>("Contracts");
            _controlPanel = _controlCenter.Q<VisualElement>("ControlPanel");
            _allContracts = Resources.Load<Contracts>("Contracts");
            _contractsView.itemsSource = _allContracts.Data.Where(x => (int)x.Planet == _planetManager.CurrentPlanet.ID).ToList();

            _contractsView.RegisterCallback<GeometryChangedEvent>(OnListViewGeometryChanged);

            _contractsView.bindItem = (element, index) =>
            {
                var contract = _allContracts.Data[index];

                element.Q<Label>("Description").text = contract.Description;
                element.SetEnabled(contract.RatingNeeded <= _playerSettings.Rating);
            };

            _statusLabel = _controlPanel.Q<Label>("StatusLabel");

            _contractsView.selectionChanged += RefreshSelectedContract;
            _contractsView.selectedIndex = 0;

            _signButton = _controlPanel.Q<Button>("SignButton");
            _signButton.clicked += SignContract;
        }

        private void OnListViewGeometryChanged(GeometryChangedEvent evt)
        {
            _contractsView.schedule.Execute(() =>
            {
                var emptyLabel = _contractsView.Q<Label>(className: "unity-list-view__empty-label");
                if (emptyLabel != null && emptyLabel.text != "Нет контрактов")
                {
                    emptyLabel.text = "Нет контрактов";
                    emptyLabel.style.color = Color.white;
                    emptyLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                    emptyLabel.style.flexGrow = 1;
                    emptyLabel.style.width = Length.Percent(100);
                    emptyLabel.style.height = Length.Percent(100);
                    emptyLabel.style.justifyContent = Justify.Center;
                    emptyLabel.style.alignItems = Align.Center;
                }
            }).StartingIn(0);
        }

        private void SignContract()
        {
            _contractManager.CurrentContract = (ContractData)_contractsView.selectedItem;
            _contractManager.CurrentContractStatus = ContractStatus.Signed;
            _contractManager.SaveData();
            _controlCenter.style.display = DisplayStyle.None;
            OnContractSigned?.Invoke();
        }

        private void RefreshSelectedContract(IEnumerable<object> selectedItems)
        {
            var contract = (ContractData)_contractsView.selectedItem;
            if (contract == null)
                return;
            _controlPanel.dataSource = contract;
            if (contract.RatingNeeded <= _playerSettings.Rating) { 
                _statusLabel.text = "Доступен";}
            else
                _statusLabel.text = $"Не доступен. Требуется рейтинг {contract.RatingNeeded} и выше.";
            if (_signButton != null)
                _signButton.SetEnabled(contract.RatingNeeded <= _playerSettings.Rating);
        }

        private void OnDestroy()
        {
            _contractsView.selectionChanged -= RefreshSelectedContract;
            _signButton.clicked -= SignContract;
        }
    }
}
