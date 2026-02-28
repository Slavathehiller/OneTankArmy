using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LifeManager : MonoBehaviour
{
    public event UnityAction AllEnemyDead;
    public event UnityAction<int> EnemyLiveCount;
    private AIEnemy[] _allEnemies;
    void Start()
    {
        _allEnemies = FindObjectsByType<AIEnemy>(FindObjectsSortMode.None);
        foreach (var enemy in _allEnemies)
            enemy.Die += EnemyDie;
        EnemyCountChanged();
    }

    private void EnemyDie(BaseEntity deadEnemy)
    {
        EnemyCountChanged();
    }
    private void EnemyCountChanged()
    {
        EnemyLiveCount?.Invoke(_allEnemies.Count(x => !x.IsDead));
        if (!_allEnemies.Any(x => !x.IsDead))
            AllEnemyDead?.Invoke();
    }


    private void OnDestroy()
    {
        foreach (var enemy in _allEnemies)
            enemy.Die -= EnemyDie;
    }
}
