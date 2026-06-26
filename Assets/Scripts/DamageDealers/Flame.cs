using Assets.Scripts.DamageDealers;
using UnityEngine;

public class Flame : DamageDealerDOT
{
    [SerializeField]
    private GameObject _flameBody;
    public void On()
    {
        _flameBody.SetActive(true);
    }

    public void Off()
    {
        _flameBody.SetActive(false);
    }

    public bool IsOn()
    {
        return _flameBody.activeSelf;
    }
}

