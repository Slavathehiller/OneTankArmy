using Assets.Player;
using Assets.Scripts.Player;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Scripts.SceneAssets
{
    public class RepairZone : MonoBehaviour
    {
        public event UnityAction<Consumables> OnConsumableUsing;

        [SerializeField]
        private UIDocument _document;

        [SerializeField]
        private ErrorController _errorController;

        [Inject]
        private IPlayerSettings _playerSettings;
        
        private VisualElement _repairZone;
        private Label _messageLabel;
        private Button _showAdvButton;
        private Button _usenanorepairButton;
        private VisualElement _advWindow;

        private void Start()
        {
            _repairZone = _document.rootVisualElement.Q<VisualElement>("RepairZoneWindow");            
            _messageLabel = _repairZone.Q<Label>("MessageLabel");
            _showAdvButton = _document.rootVisualElement.Q<Button>("ShowAdvButton");
            _usenanorepairButton = _document.rootVisualElement.Q<Button>("UseNanorepairButton");
            _advWindow = _document.rootVisualElement.Q<VisualElement>("AdvWindow");
            _advWindow.Q<Button>("CloseButton").clicked += CloseAdvWindow;
            _showAdvButton.clicked += RepairForAdv;
            _usenanorepairButton.clicked += UseNanoRepairKit;
        }

        private void Update()
        {
            if (_playerSettings.RepairEndTime != null)
            {
                var remaining = _playerSettings.RepairEndTime - DateTime.Now;
                _messageLabel.text = "До окончания ремонта: " + remaining.Value.ToString(@"hh\:mm\:ss");
            }
            else
            {
                _messageLabel.text = "Техника в порядке. Ремонт не требуется.";
            }
            ShowRepSupportButtons(_playerSettings.RepairEndTime != null);
        }

        public void RepairForAdv()
        {
            _advWindow.style.display = DisplayStyle.Flex;
            _playerSettings.RepairEndTime = DateTime.Now;
        }

        public void CloseAdvWindow()
        {
            _advWindow.style.display = DisplayStyle.None;
        }

        public void UseNanoRepairKit()
        {
            if (_playerSettings.GetConsumable(Consumables.NanoRepairKit) < 1)
                _errorController.ShowError("У вас нет ни одного наноремонтного комплекта. \nУскорение ремонта невозможно.");
            else
            {
                _playerSettings.RemoveConsumable(Consumables.NanoRepairKit);
                _playerSettings.RepairEndTime = DateTime.Now;
                OnConsumableUsing?.Invoke(Consumables.NanoRepairKit);
            }
        }


        public void ShowRepSupportButtons(bool on)
        {
            if (on)
            {
                _showAdvButton.style.display = DisplayStyle.Flex;
                _usenanorepairButton.style.display = DisplayStyle.Flex;
            }
            else
            {
                _showAdvButton.style.display = DisplayStyle.None;
                _usenanorepairButton.style.display = DisplayStyle.None;
            }
        }

        private void OnDestroy()
        {
            _showAdvButton.clicked -= RepairForAdv;
            _usenanorepairButton.clicked -= UseNanoRepairKit;
            _advWindow.Q<Button>("CloseButton").clicked -= CloseAdvWindow;
        }
    }
}
