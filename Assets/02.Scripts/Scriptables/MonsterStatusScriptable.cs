using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Monster Status", menuName = "Scriptable/Status/New Monster Status")]
public class MonsterStatusScriptable : ScriptableObject
{
    public int maxLife = 1;
    public float maxHp = 3f;

    public float restTime = 5f;

    public float ccTime = 1f;

    public float moveSpeed = 1f;

    public int attackDamage = 1;
    public float attackRange = 5f;
    public float attackDelayTime = 1f;

    public float idleDelayTime = 2f;

    public float dieLeftTime = 5f;
    
    public AudioClip damagedAudio;
    public AudioClip neckSliceAudio;
    public AudioClip dieAudio;
}
