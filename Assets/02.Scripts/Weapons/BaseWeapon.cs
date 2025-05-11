using System;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponInfoScriptable weaponInfo;

    public LayerMask hittableLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (weaponInfo.myWeaponType is WeaponInfoScriptable.WeaponType.Long
            || !other.TryGetComponent(out HittableObject hittableObject))
        {
            return;
        }
        
        OnTriggerAction(hittableObject);
    }

    protected virtual void OnTriggerAction(HittableObject hittableObject)
    {
        
    }

    public void Use()
    {
        if (weaponInfo.myWeaponType is WeaponInfoScriptable.WeaponType.Short)
        {
            return;
        }
        
        UseWeapon();
    }

    protected virtual void UseWeapon()
    {
        
    }
}
