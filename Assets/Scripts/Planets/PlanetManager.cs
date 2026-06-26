using Assets.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Planets
{
    public class PlanetManager : IPlanetManager
    {
        private PlanetInfo _currentPlanetInfo;
        public PlanetInfo CurrentPlanet => _currentPlanetInfo;

        [Inject]
        private IPlayerSettings _playerSettings;

        public void SetCurrentPlanetInfo()
        {
            var planetsInfo = Resources.Load<Planets>("Planets").Data;
            _currentPlanetInfo = planetsInfo.First(x => x.ID == _playerSettings.CurrentPlanetID);
        }
    }
}
