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
            wirePointUI.localScale = myStatus.wirePointOriginScale * Mathf.Max(1, hitInfo.distance);
            
            attachPoint = hitInfo.point;
            LineRenderer.positionCount = 2;  
            LineRenderer.SetPosition(0, position);
            LineRenderer.SetPosition(1, attachPoint);
            
            _sj = Player.gameObject.AddComponent<SpringJoint>();
            _sj.autoConfigureConnectedAnchor = false;
            _sj.anchor = Vector3.zero;
            _sj.connectedAnchor = attachPoint;
            
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
            LineRenderer.positionCount = 0;

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
        LineRenderer.SetPosition(0, WireController.HandPositionDict[myHandType]() + Vector3.down);
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
        
        SpringJoint[] springJoints = Player.GetComponents<SpringJoint>();
        for (int i = 0; i < springJoints.Length; i++)
        {
            Destroy(springJoints[i]);
        }
        LineRenderer.positionCount = 0;
        
        wirePointUI.gameObject.SetActive(false);
        
        currentType = WireType.Ready;
        Debug.Log($"상태 변경 {currentType}");
    }
}