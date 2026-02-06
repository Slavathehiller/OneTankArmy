using Assets.Scripts.DamageDealers;
using UnityEngine;

public class Flame : DamageDealerDOT
{
    public void Off() 
    {
        Destroy(gameObject);
    }
}
