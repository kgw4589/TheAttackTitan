using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WireController : MonoBehaviour
{
    public Transform teleportCircleUI;
    
    private LineRenderer _lr;
    private CharacterController _cc;

    private Vector3 _originScale = new Vector3(0.025f, 0.025f, 0.025f);

    public bool isWarp = false;
    public float warpTime = 0.2f;
    public PostProcessVolume post;

    public BaseWire leftWire;
    public BaseWire rightWire;

    private void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        
        _lr = GetComponent<LineRenderer>();
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        WireProcess();
    }

    private void WireProcess(BaseWire wire)
    {
        switch (wire.currentType)
        {
            case BaseWire.WireType.Ready :
                // wire.Shoot();
                break;
        }
    }

    private void Shoot()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            rightWire.Shoot(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
        }

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
        {
            leftWire.Shoot(ARAVRInput.RHandPosition, ARAVRInput.RHandPosition);
        }
    }
}
