using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveMonsterInfo
{
    public GameObject monster;
    public int count;
}

[Serializable]
public class WaveInfo
{
    public List<WaveMonsterInfo> monsters;
    public float waveStartDelay = 30.0f;

    public Vector2 monsterCreateTimeRange = new Vector2(1f, 3f);
}

[CreateAssetMenu(fileName = "New Wave Scriptable", menuName = "Scriptable/Wave")]
public class WaveScriptable : ScriptableObject
{
    public List<WaveInfo> waves;
}
