using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Scripts.SceneAssets
{
    public class RepairZone : MonoBehaviour
    {
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
            _usenanorepairButton.clicked += UseNanoRepaitKit;
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

        public void UseNanoRepaitKit()
        {
            _errorController.ShowError("У вас нет ни одного наноремонтного комплекта. \nУскорение ремонта невозможно.");
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
            _usenanorepairButton.clicked -= UseNanoRepaitKit;
            _advWindow.Q<Button>("CloseButton").clicked -= CloseAdvWindow;
        }
    }
}
