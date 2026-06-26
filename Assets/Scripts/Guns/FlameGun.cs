using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class FlameGun : Gun
{
    [SerializeField]
    private Flame _flame;



    protected override void UpdateActions()
    {
        if (_fireCooldown < _fireLatency)
            _fireCooldown += Time.deltaTime;
        if (_fireCooldown >= _fireLatency)
        {
            _flame.Off();
            _audioSourceFire.Stop();
        }
    }

    public override void TryFire()
    {
        _fireCooldown = 0;
        Fire();
    }


    protected override void Fire()
    {
        _flame.On();
        if (!_audioSourceFire.isPlaying)
            _audioSourceFire.Play();
    }
}

