using UnityEngine;

public class BaseWire : MonoBehaviour
{
    public Rigidbody playerRigid;
    public Transform wirePointUI;
    public LayerMask attachableLayers;

    public enum WireType
    {
        Ready, Shooting, Attaching, Collecting
    }

    public WireType currentType = WireType.Ready;
    
    [HideInInspector] public Vector3 attachPoint;

    private LineRenderer _lr;
    
    private Vector3 _wirePointOriginScale = new Vector3(0.025f, 0.025f, 0.025f);
    private float _range = 20f;
    private float _power = 50f;

    private void Initialize()
    {
        currentType = WireType.Ready;

        _lr = GetComponent<LineRenderer>();
        
        wirePointUI.gameObject.SetActive(false);
    }
    
    public virtual void Shoot(Vector3 position, Vector3 direction)
    {
        if (currentType is not WireType.Ready)
        {
            return;
        }

        currentType = WireType.Shooting;
        
        Ray ray = new Ray(position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _range, attachableLayers))
        {
            _lr.positionCount = 2;
            _lr.SetPosition(0, ray.origin + new Vector3(0, 0.05f, 0));
            _lr.SetPosition(1, hitInfo.point);

            attachPoint = hitInfo.point;
                
            wirePointUI.gameObject.SetActive(true);
            wirePointUI.position = hitInfo.point;
            wirePointUI.forward = hitInfo.normal;
            wirePointUI.localScale = _wirePointOriginScale * Mathf.Max(1, hitInfo.distance);

            currentType = WireType.Attaching;
        }
        else
        {
            _lr.positionCount = 0;

            wirePointUI.gameObject.SetActive(false);

            currentType = WireType.Ready;
        }
    }

    protected virtual void Attached()
    {
        Vector3 direction = (attachPoint - transform.position).normalized;
        Vector3 force = direction * _power * Time.deltaTime;

        playerRigid.AddForce(force, ForceMode.Force);
    }
    
    public virtual void Collect()
    {
        if (currentType is WireType.Ready)
        {
            return;
        }
        
        
    }
}
