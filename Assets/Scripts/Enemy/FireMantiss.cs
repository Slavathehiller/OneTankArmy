using Assets.Scripts.VFX.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FireMantiss : AIEnemy
{
    [SerializeField]
    protected float _distanceOfFlame;

    [SerializeField]
    protected float _cooldownOfFlame;
    private float _currentCooldownOfFlame;

    [SerializeField]
    protected float _durationOfFlame;
    private float _currentDurationOfFlame;

    [SerializeField]
    protected Transform _firePoint;

    private Flame _flame;

    private bool _isHide;

    private List<SpriteRenderer> _spriteRenderers = new();

    [Inject]
    private IVFXManager _VFXMmanager;

    protected override void ReactToDamage(DamageDealer dd)
    {
        
    }

    protected void Hiding()
    {
        StartCoroutine(HidingCoroutine(true));
    }

    private IEnumerator HidingCoroutine(bool on)
    {
        var fadeDuration = 1f;
        var elapsedTime = 0f;

        List<Color> originalColors = new ();
        foreach (var sr in _spriteRenderers)
        {
            originalColors.Add(sr.color);
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha;
            if (on)
                alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            else
                alpha = Mathf.Lerp(1f, 0f, 1 - elapsedTime / fadeDuration);

            for (int i = 0; i < _spriteRenderers.Count; i++)
            {
                if (_spriteRenderers[i] != null)
                {
                    Color c = originalColors[i];
                    _spriteRenderers[i].color = new Color(c.r, c.g, c.b, alpha);
                }
            }

            yield return null;
        }


        foreach (var sr in _spriteRenderers)
        {
            if (sr != null)
                if (on)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
                else
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        }
        _isHide = on;
    }


    protected void Revealing()
    {
        StartCoroutine(HidingCoroutine(false));
    }

    protected void Flame()
    {
        _flame = _VFXMmanager.MeakeVFXAt<Flame>(Vector3.zero);
        _flame.transform.SetParent(_firePoint);        
        _flame.transform.localRotation = Quaternion.identity;
        _flame.transform.localPosition = Vector3.zero;

        _currentDurationOfFlame = _durationOfFlame;
    }

    protected void Slash()
    {
        _animator.SetBool("Slashing", true);
    }

    public void EndSlash()
    {
        if (_inContactWithTarget)
            _target.GetComponent<TankController>().TakeDamage(_meleeDamage);
        _animator.SetBool("Slashing", false);
        _currentMeleeAttackCooldown = _meleeAttackCooldown;
    }

    protected override void UpdateActions()
    {
        if (_currentCooldownOfFlame > 0)
            _currentCooldownOfFlame -= Time.deltaTime;
        if (_currentDurationOfFlame > 0)
            _currentDurationOfFlame -= Time.deltaTime;
        base.UpdateActions();
    }

    protected override void StartActions()
    {
        base.StartActions();
        _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>(true));
    }

    private void FixedUpdate()
    {
        if (_currentDurationOfFlame > 0)
        {
            return;
        }
        else
        {
            if (_flame != null)
            {
                _flame.Off();
                _flame = null;
            }
        }

        if (_isDead)
            return;

        if (_target == null)
        {
            _animator.SetBool("Moving", false);
            if (!_isHide)
                Hiding();
            return;
        }

        if (_isHide)
            Revealing();

        if (!TryToRotateAtTarget())
        {
            RigidBody.angularVelocity = 0;
            if (Vector3.Distance(transform.position, _target.transform.position) <= _distanceOfFlame && _currentCooldownOfFlame <= 0)
            {
                StopMoving();
                _currentCooldownOfFlame = _cooldownOfFlame;
                Flame();
                return;
            }
            if (_inContactWithTarget && _currentMeleeAttackCooldown <= 0)
            {
                StopMoving();
                Slash();
                return;
            }
            if (!_inContactWithTarget)
                MoveToTarget();
            return;
        }
    }

    protected override void DeadPerfomance()
    {       
        if (_flame != null)
            _flame.Off();
        StopAllCoroutines();
        Revealing();
        base.DeadPerfomance();
        DisablePhysic();
        StartCoroutine(MakeGoooCoroutine<BigGreenGoo>(BodyParts[0].gameObject));
    }
}
