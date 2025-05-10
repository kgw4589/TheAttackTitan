using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableObject : MonoBehaviour, IDamagable
{
    public enum HitType
    {
        LegSlice,
        ArmSlice,
        NeckSlice,
        BodyScratch
    }

    public HitType myHitType = HitType.BodyScratch;

    public void DamageAction(int damage, Vector3 hitPoint, Vector3 normal)
    {
        switch (myHitType)
        {
            case HitType.NeckSlice :
                break;
            
            case HitType.BodyScratch :
                break;
        }
    }
}
