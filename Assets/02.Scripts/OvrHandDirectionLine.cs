using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvrHandDirectionLine : MonoBehaviour
{
    public WireStatusScriptable wireStatusScriptable;
    private LineRenderer _directionLineRenderer;

    private void Start()
    {
        _directionLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        
    }
}
