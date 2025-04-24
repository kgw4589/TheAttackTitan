using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;
    
    private LineRenderer _lr;
    private CharacterController _cc;

    private Vector3 _originScale = new Vector3(0.025f, 0.025f, 0.025f);

    public bool isWarp = false;
    public float warpTime = 0.2f;
    public PostProcessVolume post;

    private void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        
        _lr = GetComponent<LineRenderer>();
        _cc = GetComponent<CharacterController>();
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
                if (!isWarp)
                {
                    _cc.enabled = false;
                    transform.position = teleportCircleUI.position + Vector3.up;
                    _cc.enabled = true;
                }
                else
                {
                    StartCoroutine(Warp());
                }
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;

            int layer = 1 << LayerMask.NameToLayer("Terrain");

            if (Physics.Raycast(ray, out hitInfo, 200f, layer))
            {
                _lr.SetPosition(0, ray.origin + new Vector3(0, 0.05f, 0));
                _lr.SetPosition(1, hitInfo.point);
                
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                teleportCircleUI.forward = hitInfo.normal;
                teleportCircleUI.localScale = _originScale * Mathf.Max(1, hitInfo.distance);
            }
            else
            {
                _lr.SetPosition(0, ray.origin + new Vector3(0, 0.05f, 0));
                _lr.SetPosition(1, ray.origin + ray.direction * 200f);

                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator Warp()
    {
        MotionBlur blur;

        Vector3 pos = transform.position;
        Vector3 targetPos = teleportCircleUI.position + Vector3.up;

        float currentTime = 0;

        post.profile.TryGetSettings<MotionBlur>(out blur);

        blur.active = true;
        _cc.enabled = false;

        while (currentTime < warpTime)
        {
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);

            yield return null;
        }

        transform.position = teleportCircleUI.position + Vector3.up;
        _cc.enabled = true;
        blur.active = false;
    }
}
