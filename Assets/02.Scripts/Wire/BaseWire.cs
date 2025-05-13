using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class BaseWire : MonoBehaviour
{
    public WireController.HandType myHandType;
    public WireStatusScriptable myStatus;
    public Transform wirePointUI;

    public Transform directionPoint;
    public LineRenderer directionLine;

    [HideInInspector] public Vector3 attachPoint;

    public enum WireType
    {
        Ready, Shooting, Attaching, Collecting
    }

    public WireType currentType = WireType.Ready;

    protected Transform player;
    protected LineRenderer lineRenderer;

    public virtual void Initialize(Transform player)
    {
        currentType = WireType.Ready;

        this.player = player;
        lineRenderer = GetComponent<LineRenderer>();
        
        wirePointUI.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        Ray ray = new Ray(WireController.HandPositionDict[myHandType](),
                        WireController.HandDirectionDict[myHandType]());
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, myStatus.range, myStatus.attachableLayers))
        {
            directionPoint.gameObject.SetActive(true);
            directionPoint.position = hitInfo.point;
            directionPoint.forward = hitInfo.normal;
        }
        else
        {
            directionPoint.gameObject.SetActive(false);
        }

        if (currentType is WireType.Ready)
        {
            Vector3 startPos = WireController.HandPositionDict[myHandType]();
            Vector3 dir = WireController.HandDirectionDict[myHandType]();
            Vector3 endPos = startPos + dir * myStatus.range;

            directionLine.positionCount = 2;
            directionLine.SetPosition(0, startPos);
            directionLine.SetPosition(1, endPos);
        }
        else
        {
            directionPoint.gameObject.SetActive(false);
            directionLine.positionCount = 0;
        }
    }
    
    public virtual void Shoot()
    {
        
    }

    public virtual void Attached()
    {
        
    }
    
    public virtual void Collect()
    {
        
    }
}
