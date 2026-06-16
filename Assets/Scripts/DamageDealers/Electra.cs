using Assets.Scripts.DamageDealers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electra : DamageDealerDOT
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseEntity>(out var target) && target.TagCloud.Contains(Tag.Insect))
            target.InstantDeath();
    }
}
