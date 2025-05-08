using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WireController : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right
    }

    // 딕셔너리 사용: HandType -> (Position, Direction) 쌍
    public static readonly Dictionary<HandType, System.Func<ARAVRInput.Controller>> HandControllerDict = new()
    {
        { HandType.Left, () => ARAVRInput.Controller.LTouch },
        { HandType.Right, () => ARAVRInput.Controller.RTouch }
    };
    
    public static readonly Dictionary<HandType, System.Func<Vector3>> HandPositionDict = new()
    {
        { HandType.Left, () => ARAVRInput.LHandPosition },
        { HandType.Right, () => ARAVRInput.RHandPosition }
    };

    public static readonly Dictionary<HandType, System.Func<Vector3>> HandDirectionDict = new()
    {
        { HandType.Left, () => ARAVRInput.LHandDirection },
        { HandType.Right, () => ARAVRInput.RHandDirection }
    };

    public PostProcessVolume post;

    public BaseWire leftWire;
    public BaseWire rightWire;

    private void Start()
    {
        leftWire.Initialize(transform);
        rightWire.Initialize(transform);
    }

    private void Update()
    {
        WireProcess(rightWire);
        WireProcess(leftWire);
    }

    private void WireProcess(BaseWire wire)
    {
        switch (wire.currentType)
        {
            case BaseWire.WireType.Ready :
                wire.Shoot();
                break;
            
            case BaseWire.WireType.Attaching :
                wire.Attached();
                break;
        }
    }
}
