using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWire : BaseWire
{
    private SpringJoint _sj;
    
    public override void Shoot()
    {
        Vector3 handPosition = WireController.HandPositionDict[myHandType]();
        Vector3 handDirection = WireController.HandDirectionDict[myHandType]();
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, WireController.HandControllerDict[myHandType]()))
        {
            currentType = WireType.Shooting;
            ShootWire(handPosition, handDirection);
        }
    }

    private void ShootWire(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, myStatus.range, myStatus.attachableLayers))
        {
            wirePointUI.gameObject.SetActive(true);
            wirePointUI.position = hitInfo.point;
            wirePointUI.forward = hitInfo.normal;
            
            attachPoint = hitInfo.point;
            lineRenderer.positionCount = 2;  
            lineRenderer.SetPosition(0, position + new Vector3(0, 2, 0));
            lineRenderer.SetPosition(1, attachPoint);
            
            _sj = player.gameObject.AddComponent<SpringJoint>();
            
            _sj.autoConfigureConnectedAnchor = false;
            _sj.anchor = Vector3.zero;

            if (hitInfo.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                _sj.connectedBody = rigidbody;
                Vector3 localAnchor = rigidbody.transform.InverseTransformPoint(hitInfo.point);
                _sj.connectedAnchor = localAnchor;
            }
            else
            {
                _sj.connectedAnchor = attachPoint;
            }
            
            _sj.maxDistance = myStatus.maxDistance;
            _sj.minDistance = myStatus.minDistance;
            _sj.spring = myStatus.spring;
            _sj.damper = myStatus.damper;
            _sj.breakForce = myStatus.breakForce;

            currentType = WireType.Attaching;
            Debug.Log($"상태 변경 {currentType}");
        }
        else
        {
            lineRenderer.positionCount = 0;

            wirePointUI.gameObject.SetActive(false);

            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(myStatus.shootDelay);
        
        currentType = WireType.Ready;
        Debug.Log($"상태 변경 {currentType}");
    }
    
    public override void Attached()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, WireController.HandControllerDict[myHandType]()))
        {
            Collect();
        }
        else
        {
            DrawRope();
        }
    }
    
    private void DrawRope()
    {
        lineRenderer.SetPosition(0, WireController.HandPositionDict[myHandType]() + Vector3.down);

        if (_sj?.connectedBody)
        {
            Vector3 connectedWorldPos = _sj.connectedBody.transform.TransformPoint(_sj.connectedAnchor);
            lineRenderer.SetPosition(1, connectedWorldPos);
            wirePointUI.transform.position = connectedWorldPos;
        }
        //
        // if (_tankInput.OnRightMouseDown && !_isDash)
        // {
        //     _playerRigid.velocity = Vector3.zero;
        //     _isDash = true;
        // }
    }

    public override void Collect()
    {
        currentType = WireType.Collecting;
        Debug.Log($"상태 변경 {currentType}");
        
        SpringJoint[] springJoints = player.GetComponents<SpringJoint>();
        for (int i = 0; i < springJoints.Length; i++)
        {
            Destroy(springJoints[i]);
        }
        lineRenderer.positionCount = 0;
        
        wirePointUI.gameObject.SetActive(false);
        
        currentType = WireType.Ready;
        Debug.Log($"상태 변경 {currentType}");
    }
}