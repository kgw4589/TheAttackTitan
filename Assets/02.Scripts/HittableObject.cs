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
        _iTitan = transform.root.GetComponent<ITitan>();
    }

    public void DamageAction(int damage, Vector3 hitPoint, Vector3 normal)
    {
        switch (myHitType)
        {
            case HitType.NeckSlice :
                _iTitan.SliceNeck(hitPoint, normal);
                break;
            
            case HitType.BodyScratch :
                _iTitan.ScratchBody(hitPoint, normal);
                break;
        }
    }
    
    private void OtherDamageAction()
    {
        
    }
}
