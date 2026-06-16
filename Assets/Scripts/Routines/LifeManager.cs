using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LifeManager : MonoBehaviour
{
    public event UnityAction AllEnemyDead;
    public event UnityAction<int> EnemyLiveCount;
    private List<AIEnemy> _allEnemies;
    public void Init()
    {
        _allEnemies = FindObjectsByType<AIEnemy>(FindObjectsSortMode.None).ToList();
        foreach (var enemy in _allEnemies)
            enemy.Die += EnemyDie;
        EnemyCountChanged();
    }

    public void AddEnemy(AIEnemy enemy)
    {
        _allEnemies.Add(enemy);
        enemy.Die += EnemyDie;
    }

    private void EnemyDie(BaseEntity deadEnemy)
    {
        EnemyCountChanged();
    }

    public int EnemyLiveNow()
    {
        return _allEnemies.Count(x => !x.IsDead);
    }

    private void EnemyCountChanged()
    {
        EnemyLiveCount?.Invoke(EnemyLiveNow());
        if (!_allEnemies.Any(x => !x.IsDead))
            AllEnemyDead?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (var enemy in _allEnemies)
            enemy.Die -= EnemyDie;
    }
}
