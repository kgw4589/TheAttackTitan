#define Remote
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스 입력에 따라 카메라를 회전 시키고 싶다.
// 필요속성 : 현재각도, 마우스감도
public class CamRotate : MonoBehaviour
{
    private const float Sensitivity = 200;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        float x = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch);
        float y = ARAVRInput.GetAxis("Vertical", ARAVRInput.Controller.RTouch);

        Vector3 angle = new Vector3(-y, x, 0);

        if (transform.rotation.x is >= 90 or <= -90)
        {
            angle.y = 0;
        }
        
        transform.Rotate(angle * (Sensitivity * Time.deltaTime));
    }
}
