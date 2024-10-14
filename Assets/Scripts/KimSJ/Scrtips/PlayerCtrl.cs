using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float Speed = 2.5f;
    private float rotateSpeed = 5.0f;
    private float limitAngle = 60.0f;

    public static int LStack = 0;
    public static int DStack = 1;

    private float mouseX, mouseY;
    private Rigidbody rb;

    public static Camera[] Cams = null;
    private int CamCount = 0;
    private int CamCurrent = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cams = GetComponentsInChildren<Camera>();

        mouseX = transform.rotation.eulerAngles.y;
        mouseY = transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        Move();
        Rotate();
        ChangeCam();
        CursorCtrl();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        movement = movement.normalized * (Time.deltaTime * Speed);
        transform.position += movement;

        mouseX += Input.GetAxis("Mouse X") * rotateSpeed;
        mouseY = Mathf.Clamp(mouseY + Input.GetAxis("Mouse Y") * rotateSpeed, -limitAngle, limitAngle);

        transform.rotation = Quaternion.Euler(transform.rotation.x - mouseY, transform.rotation.y + mouseX, 0.0f);
    }

    private void ChangeCam()
    {
        CamCount = Cams.Length;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ++CamCurrent;

            if (CamCurrent >= CamCount)
            {
                CamCurrent = 0;
            }

            for (int i = 0; i < Cams.Length; ++i)
            {
                Cams[i].enabled = (i == CamCurrent);
            }
        }
    }

    private void CursorCtrl()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}