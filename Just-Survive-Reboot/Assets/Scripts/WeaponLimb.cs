using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLimb : EnemyLimb
{
    protected override void DamageEffect(Enemy parent) {
        base.DamageEffect(parent);
        //add a temporary slow
    }

    protected override void DestructionEffect(Enemy parent) {
        base.DestructionEffect(parent);
        parent.ApplyModifiers(hitStun: 0.7f, damage: 0.8f, attackSpeed: 2);
    }
}
