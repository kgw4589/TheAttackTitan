using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpPower = 70f;
    
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        
        _rigidbody.MovePosition(transform.position + dir * (speed * Time.deltaTime));
    }
}
