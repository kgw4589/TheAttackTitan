using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI;
    
    private LineRenderer _lr;
    private CharacterController _cc;
    private Vector3 _originScale = Vector3.one * 0.02f;

    public int lineSmooth = 40;
    public float curveLength = 50f;
    public float gravity = -60f;
    public float simulateTime = 0.02f;

    private List<Vector3> _lines = new List<Vector3>();

    private void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        
        _lr = GetComponent<LineRenderer>();
        _cc = GetComponent<CharacterController>();
        
        _lr.startWidth = 0.0f;
        _lr.endWidth = 0.2f;
    }

    private void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            _lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            _lr.enabled = false;

            if (teleportCircleUI.gameObject.activeSelf)
            {
                _cc.enabled = false;

                transform.position = teleportCircleUI.position + Vector3.up;

                _cc.enabled = true;
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }
    }

    private void MakeLines()
    {
        _lines.RemoveRange(0, _lines.Count);
        // _lines.Clear();

        Vector3 dir = ARAVRInput.LHandDirection * curveLength;
        Vector3 pos = ARAVRInput.LHandPosition;

        _lines.Add(pos);

        for (int i = 0; i < lineSmooth; i++)
        {
            Vector3 lastPos = pos;

            dir.y += gravity * simulateTime;
            pos += dir * simulateTime;

            if (CheckHitRay(lastPos, ref pos))
            {
                _lines.Add(pos);
                break;
            }
            else
            {
                teleportCircleUI.gameObject.SetActive(false);
            }

            _lines.Add(pos);
        }

        _lr.positionCount = _lines.Count;
        _lr.SetPositions(_lines.ToArray());
    }

    private bool CheckHitRay(Vector3 lastPos, ref Vector3 pos)
    {
        Vector3 rayDir = pos - lastPos;
        Ray ray = new Ray(lastPos, rayDir);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayDir.magnitude))
        {
            pos = hitInfo.point;

            int layer = LayerMask.NameToLayer("Terrain");

            if (hitInfo.transform.gameObject.layer == layer)
            {
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.transform.position = hitInfo.point;
                teleportCircleUI.forward = hitInfo.normal;

                float distance = (pos - ARAVRInput.LHandPosition).magnitude;
                teleportCircleUI.localScale = _originScale * Mathf.Max(1, distance);
            }

            return true;
        }
        
        return false;
    }
}
