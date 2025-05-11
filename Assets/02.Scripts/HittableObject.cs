using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour, IDamagable
{
    public enum HitType
    {
        LegSlice,
        ArmSlice,
        NeckSlice,
        BodyScratch,
        Other
    }

    public HitType myHitType = HitType.BodyScratch;
    
    private ITitan _iTitan;

    private void Awake()
    {
        _iTitan = GetComponent<ITitan>();
    }

    public void DamageAction(int damage, Vector3 hitPoint, Vector3 normal)
    {
        switch (myHitType)
        {
            case HitType.NeckSlice :
                _iTitan.SliceNeck();
                break;
            
            case HitType.BodyScratch :
                _iTitan.ScratchBody();
                break;
        }
    }
    
    private void OtherDamageAction()
    {
        
    }
}
