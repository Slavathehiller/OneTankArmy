using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class NM_Devourer : NocturneMachine
    {
        [SerializeField]
        private Flame _desintegrator;

        [SerializeField]
        private GameObject _explosion;

        [SerializeField]
        protected float _durationOfFlame = 2f;
        private float _currentDurationOfFlame;

        protected override void Fire()
        {
            _desintegrator.On();
            _currentDurationOfFlame = _durationOfFlame;

            if (_missileSound != null)
                _missileSound.Play();
        }

        protected override void UpdateActions()
        {
            if (_currentDurationOfFlame <= 0)
            {
                base.UpdateActions();
                return;
            }

            _currentDurationOfFlame -= Time.deltaTime;

            if (_currentDurationOfFlame <= 0)
            {
                _desintegrator.Off();
                if (_missileSound != null)
                    _missileSound.Stop();
            }
        }

        protected override void FixedUpdateActions()
        {
            if (_currentDurationOfFlame <= 0)
                base.FixedUpdateActions();
        }

        protected override void DeadPerfomance()
        {
            base.DeadPerfomance();
            Explode();
            Invoke("DisablePhysic", 2f);
        }

        private void Explode()
        {
            _mainBody.SetActive(false);
            _explosion.SetActive(true);
        }
    }
}
