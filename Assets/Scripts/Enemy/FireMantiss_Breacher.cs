using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;


public class FireMantiss_Breacher : FireMantiss
{
    public event UnityAction<AIEnemy> CrossBreachLine;
    private GameObject[] _breachPoints;

    protected Transform _downBreachLine;

    protected override void StartActions()
    {
        base.StartActions();
        MoveToBreachPoint();
    }

    protected override void UpdateActions()
    {
        if (transform.position.y < _downBreachLine.transform.position.y)
            CrossBreachLine?.Invoke(this);
        base.UpdateActions();
    }

    public void BindBreachPoints(GameObject[] breachPoints)
    {
        _breachPoints = breachPoints;
    }

    public void BindBreachLine(Transform downBreachLine)
    {
        _downBreachLine = downBreachLine;
    }

    protected override void LooseEnemy()
    {
        if (_target.GetComponent<PlayerSide>() != null)
            base.LooseEnemy();
        if (_target == null)
        {
            MoveToBreachPoint();
        }
    }

    private void MoveToBreachPoint()
    {
        _target = NearestBreachPoint;
        MoveToTarget();
    }

    //protected override void DetectEnemy(PlayerSide player)
    //{
    //    if (player.IsDead)
    //        return;
    //    base.DetectEnemy(player);
    //}

    private GameObject NearestBreachPoint 
    {  
        get 
        { 
            return _breachPoints?.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
        } 
    }
}

