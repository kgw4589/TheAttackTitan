using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKatana : BaseWeapon
{
    protected override void OnTriggerAction(HittableObject hittableObject)
    {
        hittableObject.DamageAction(weaponInfo.damage, transform.position, Vector3.zero);
    }
}
