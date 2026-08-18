using Assets.Scripts.DamageDealers;
using UnityEngine;

public class WirlMissile : DamageDealerDOT
{
    protected override void DoDotDamage(BaseEntity entity)
    {
        if (entity.TagCloud.Contains(Tag.WirlCannonOperator))
            return;
        base.DoDotDamage(entity);
    }
}
