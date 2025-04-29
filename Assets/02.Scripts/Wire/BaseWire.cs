using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class BaseWire : MonoBehaviour
{
    public WireController.HandType myHandType;
    public WireStatusScriptable myStatus;
    public Transform wirePointUI;
    
    [HideInInspector] public Vector3 attachPoint;

    public enum WireType
    {
        Ready, Shooting, Attaching, Collecting
    }

    public WireType currentType = WireType.Ready;

    protected Transform Player;
    protected Rigidbody PlayerRigid;
    protected LineRenderer LineRenderer;

    public virtual void Initialize(Transform player)
    {
        currentType = WireType.Ready;

        Player = player;
        PlayerRigid = player.GetComponent<Rigidbody>();
        LineRenderer = GetComponent<LineRenderer>();
        
        wirePointUI.gameObject.SetActive(false);
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
