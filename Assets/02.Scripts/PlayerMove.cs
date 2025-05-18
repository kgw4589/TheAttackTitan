using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _speed = 25f;
    private float _jumpPower = 13f;

    private Rigidbody _rigidbody;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 입력 값
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        // 카메라의 Y축 회전만 고려
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        // Y축 방향은 무시하고 수평 방향만 사용
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // 이동 방향 계산 (카메라의 회전 방향에 따라)
        Vector3 movement = cameraForward * v + cameraRight * h;

        // 점프 처리
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }

        // 이동 처리
        _rigidbody.MovePosition(transform.position + movement * (_speed * Time.deltaTime));
    }
}