using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Scriptable", menuName = "Scriptable/Weapon")]
public class WeaponInfoScriptable : ScriptableObject
{
    public enum WeaponType
    {
        Short,
        Long,
    }

    public WeaponType myWeaponType = WeaponType.Short;

    public int damage = 1;
    
    public float range = 25f;
    public float shootDelay = 1f;
    public int maxAmmo = 30;
}
