using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cam;
    Vector2 turnVec;
    public float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        turnVec.x += Input.GetAxis("Mouse X") * sensitivity;
        turnVec.y += Input.GetAxis("Mouse Y") * sensitivity;
        cam.localRotation = Quaternion.Euler(-turnVec.y, 0, 0);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, turnVec.x, transform.localEulerAngles.z);
    }
}
