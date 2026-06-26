using Assets.Scripts.DamageDealers;
using UnityEngine;

public class Flame_old : DamageDealerDOT
{
    public void Off() 
    {
        Destroy(gameObject);
    }
}
