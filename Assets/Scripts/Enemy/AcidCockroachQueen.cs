using System;
using UnityEngine;

public class AcidCockroachQueen : AIEnemy
{
    public event Action Spawn;


    [SerializeField]
    public Transform SpawnPoint;

    [SerializeField]
    private float _spawnPeriod = 2f;

    private float _spawnTimer = 0;


    protected override void UpdateActions()
    {
        base.UpdateActions();
        if (_spawnTimer > 0)
            _spawnTimer -= Time.deltaTime;
        if (!IsDead && _target != null && _spawnTimer <= 0)
            MakeSpawn();
    }

    protected override void InitTagCloud()
    {
        base.InitTagCloud();
        TagCloud.Add(Tag.Insect);
    }

    private void MakeSpawn()
    {
        _animator.SetTrigger("Spawn");
        _spawnTimer = _spawnPeriod;
    }

    protected override void ReactToDamage(DamageDealer dd)
    {
        
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
        DisablePhysic();
        StartCoroutine(MakeGoooCoroutine<BigAcidGoo>(BodyParts[0].gameObject, 5));
    }


    public void SpawnCockroach() 
    {
        Spawn?.Invoke();
    }

}
