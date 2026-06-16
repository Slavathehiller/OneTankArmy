using NaughtyAttributes;
using UnityEngine;

public class ElectroFence : MonoBehaviour
{
    [SerializeField]
    private Electra _electra;
    
    [SerializeField]
    [BoxGroup("Damage Settings")]
    private bool _damaged;

    [SerializeField]
    [ShowIf("_damaged")]
    [BoxGroup("Damage Settings")]
    private float _breakingPeriod;

    [SerializeField]
    [ShowIf("_damaged")]
    [BoxGroup("Damage Settings")]
    private float _breakingDuration;

    [SerializeField]
    [ShowIf("_damaged")]
    [BoxGroup("Damage Settings")]
    [Range(0.01f, 1)]
    private float _breakingChance = 0.5f;


    private float _workContinued;
    private float _timeFromBreak;


    public void On()
    {
        _electra.gameObject.SetActive(true);
    }

    public void Off()
    {
        _electra.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_damaged)
            return;

        if (_timeFromBreak < _breakingDuration)
        {
            _timeFromBreak += Time.deltaTime;
            if (_timeFromBreak >= _breakingDuration)
            {
                On();
                _workContinued = 0;
            }
        }
        else
        {
            _workContinued += Time.deltaTime;
            if (_workContinued >= _breakingPeriod)
            {
                TryBreak();
            }
        }
    }

    private void TryBreak()
    {
        if (Random.Range(0f, 1) <= _breakingChance)
        {
            Off();
            _timeFromBreak = 0;
        }
        _workContinued = 0;
    }
}
