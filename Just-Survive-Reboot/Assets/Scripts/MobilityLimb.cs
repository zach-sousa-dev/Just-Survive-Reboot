using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityLimb : EnemyLimb
{
    protected override void DamageEffect(Enemy parent) {
        base.DamageEffect(parent);
        //add a temporary slow
    }

    protected override void DestructionEffect(Enemy parent) {
        base.DestructionEffect(parent);
        parent.ApplyModifiers(speed: 0.5f, hitStun: 0.9f);
    }
}
