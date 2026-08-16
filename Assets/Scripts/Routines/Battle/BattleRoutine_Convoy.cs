using Assets.Scripts.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Routines.Battle
{
    public abstract class BattleRoutine_Convoy : BattleRoutine
    {

        private Label _civilianTotalCounter;
        private Label _civilianDestroyedCounter;
        private Label _civilianBreakedCounter;

        protected NPC_ConvoyMember[] _convoyMembers;
        protected override void ContractConditionsInit()
        {
            _convoyMembers = FindObjectsByType<NPC_ConvoyMember>(FindObjectsSortMode.None).ToArray();
            foreach (var member in _convoyMembers)
            {
                member.EscapePointReach += CheckConvoyMembersStatus;
                member.Die += CheckConvoyMembersStatus;
            }
            _document.rootVisualElement.Q<VisualElement>("ConvoyStatsPanel").style.display = DisplayStyle.Flex;
            _civilianTotalCounter = _document.rootVisualElement.Q<Label>("CivilianTotalLabel");
            _civilianDestroyedCounter = _document.rootVisualElement.Q<Label>("CivilianDestroyedLabel");
            _civilianBreakedCounter = _document.rootVisualElement.Q<Label>("CivilianBreakedLabel");

            _civilianTotalCounter.text = _convoyMembers.Count().ToString();
        }

        private void CheckConvoyMembersStatus(BaseEntity _convoyMember)
        {
            _civilianDestroyedCounter.text = _convoyMembers.Where(x => x.IsDead).Count().ToString();
            _civilianBreakedCounter.text = _convoyMembers.Where(x => x.Escaped).Count().ToString();
            if (_convoyMembers.All(x => x.IsDead || x.Escaped))
                CompleteContract();
            if (_convoyMembers.All(x => x.IsDead))
                FailContract();
        }

        protected override void OnDestroyAction()
        {
            foreach (var member in _convoyMembers)
            {
                member.EscapePointReach -= CheckConvoyMembersStatus;
                member.Die -= CheckConvoyMembersStatus;
            }
            base.OnDestroyAction();
        }
    }
}
