using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Wire Status", menuName = "Scriptable/Status/New Wire Status")]
public class WireStatusScriptable : ScriptableObject
{
    public LayerMask attachableLayers;
    public Vector3 wirePointOriginScale = new Vector3(0.025f, 0.025f, 0.025f);
    public float range = 20f;
    public float power = 50f;
    public float shootDelay = 1f;

    [Header("SpringJoint Value")]
    public float maxDistance = 1f;
    public float minDistance = 1f;
    public float spring = 100f;
    public float damper = 5f;
    public float breakForce = 10000f;
}
