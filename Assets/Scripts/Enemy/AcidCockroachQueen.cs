using UnityEngine;

public class AcidCockroachQueen : AIEnemy
{
    protected override void ReactToDamage(DamageDealer dd)
    {
        
    }

    protected override void DeadPerfomance()
    {
        base.DeadPerfomance();
        DisablePhysic();
        StartCoroutine(MakeGoooCoroutine<BigAcidGoo>(BodyParts[0].gameObject, 5));
    }


}
