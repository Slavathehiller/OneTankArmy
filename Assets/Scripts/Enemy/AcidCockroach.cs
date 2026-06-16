using Assets.Scripts.Enemy;
using UnityEngine;

public class AcidCockroach : AIRangedEnemy
{
    private float _shokedTime;
    protected bool IsShocked => _shokedTime > 0;

    protected override void FixedUpdateActions()
    {

        if (_isDead && IsShocked)
            return;

        if (_target == null)
        {
            _animator.SetBool("Moving", false);
            return;
        }

        base.FixedUpdateActions();
    }

    protected override void UpdateActions()
    {
        _shokedTime -= Time.deltaTime;
        base.UpdateActions();
    }

    protected override void InitTagCloud()
    {
        base.InitTagCloud();
        TagCloud.Add(Tag.Small)
                .Add(Tag.Insect);
    }

    private void AcidSpit()
    {
        if (_target != null && !_isDead) 
        {
            _animator.SetTrigger("AcidSpit");
        }
        _currentRangeAttackCooldown = _rangeAttackCooldown;
    }

    public void AcidSpitStart()
    {
        if (!_isDead)
            Fire();
    }

    public void GetShocked()
    {
        _shokedTime = 1;
        _animator.SetTrigger("Shoke");
        if (_agent.enabled)
            _agent.ResetPath();
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        if (_shokedTime <= 0)
        {
            GetShocked();
        }
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
       // DisablePhysic();
        StartCoroutine(MakeGoooCoroutine<AcidGoo>(BodyParts[0].gameObject, 1, DisablePhysic));
    }

    protected override void RangedAttack()
    {
        AcidSpit();
    }
}
