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

        // 회전 델타 계산
        float deltaX = x * Sensitivity * Time.deltaTime;
        float deltaY = -y * Sensitivity * Time.deltaTime;

        // 쿼터니언으로 회전 적용 (Z축 회전 제외)
        Quaternion yaw = Quaternion.AngleAxis(deltaX, Vector3.up);    // Y축 회전
        Quaternion pitch = Quaternion.AngleAxis(deltaY, Vector3.right); // X축 회전

        transform.rotation = yaw * transform.rotation;
        transform.rotation = transform.rotation * pitch;
    }
}
