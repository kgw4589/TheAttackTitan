using UnityEngine;

public interface IDamagable
{
    public void DamageAction(int damage, Vector3 hitPoint, Vector3 normal);
}
