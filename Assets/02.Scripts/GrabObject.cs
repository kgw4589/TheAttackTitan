using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private bool _isGrabbing = false;
    private GameObject _grabbedObject;

    public LayerMask grabbedLayer;
    public float grabRange = 0.2f;

    private Vector3 _prevPos;
    public float throwPower = 80f;

    private Quaternion _prevRot;
    public float rotPower = 5f;

    public bool isRemoteGrab = true;

    public float remoteGrabDistance = 20f;
    
    private void Update()
    {
        if (!_isGrabbing)
        {
            TryGrab();
        }
        else
        {
            TryUnGrab();
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            if (isRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;
                
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance))
                {
                    _isGrabbing = true;
                    _grabbedObject = hitInfo.transform.gameObject;

                    StartCoroutine(GrabbingAnimation());

                    return;
                }
            }
            
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);

            int closest = 0;

            for (int i = 0; i < hitObjects.Length; i++)
            {
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos, ARAVRInput.RHandPosition);

                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);

                if (nextDistance < closestDistance)
                {
                    closest = i;
                }
            }

            if (hitObjects.Length > 0)
            {
                _isGrabbing = true;

                _grabbedObject = hitObjects[closest].gameObject;
                _grabbedObject.transform.parent = ARAVRInput.RHand;
                _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                _prevPos = ARAVRInput.RHandPosition;
                _prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    private IEnumerator GrabbingAnimation()
    {
        float currentTime = 0f;
        float finishTime = 0.2f;

        if (_grabbedObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
        
        // _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        _prevPos = ARAVRInput.RHandPosition;
        _prevRot = ARAVRInput.RHand.rotation;

        Vector3 startLocation = _grabbedObject.transform.position;
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float elapsedRate = currentTime / finishTime;
        while (elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            _grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            
            yield return null;
        }

        _grabbedObject.transform.position = targetLocation;
        _grabbedObject.transform.parent = ARAVRInput.RHand;
    }

    private void TryUnGrab()
    {
        Vector3 throwDirection = (ARAVRInput.RHandPosition - _prevPos);
        _prevPos = ARAVRInput.RHandPosition;
        
        // 쿼터니언 공식
        // angle = Q1, angle2 = Q2
        // angle1 + angle2 = Q1 *Q2
        // -angle = Quaternion.Inverse(Q2)
        // angle2 - angle1 = Quaternion.FroToRatation(Q1, Q2) = Q2 * Quaternion.Inverse(Q1)
        // 회전 방향 = current -previous의 차로 구함. 0previous는 Inverse로 구함
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(_prevRot);
        _prevRot = ARAVRInput.RHand.rotation;
        
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            _isGrabbing = false;

            Rigidbody grabbedRigid = _grabbedObject.GetComponent<Rigidbody>();
            
            grabbedRigid.isKinematic = false;
            _grabbedObject.transform.parent = null;

            grabbedRigid.velocity = throwDirection * throwPower;
            
            // 각속도 = (1/dt) * dθ (특정 축 기준 변위 각도)
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedRigid.angularVelocity = angularVelocity;
            
            _grabbedObject = null;
        }
    }
}
