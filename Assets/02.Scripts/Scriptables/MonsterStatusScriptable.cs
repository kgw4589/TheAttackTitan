using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Monster Status", menuName = "Scriptable/Status/New Wire Status")]
public class MonsterStatusScriptable : ScriptableObject
{
    public float maxHp = 3f;

    public float moveSpeed = 1f;

    public int attackDamage = 1;
    public float attackRange = 3f;
    public float attackDelayTime = 1f;

    public float idleDelayTime = 2f;
}
