using Assets.Scripts.Planets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlanetManager
{
    public PlanetInfo CurrentPlanet { get; }
    public void SetCurrentPlanetInfo();
}

