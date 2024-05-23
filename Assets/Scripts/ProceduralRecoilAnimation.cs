using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoilAnimation : MonoBehaviour
{
    public Vector3 recoilOffset = new Vector3(0, 0, -0.1f); // Offset for the gun's position when recoiling
    public Vector3 recoilRotation = new Vector3(-10, 0, 0); // Rotation for the gun when recoiling
    public float recoilSpeed = 10f; // Speed at which the gun moves to the recoil position
    public float returnSpeed = 20f; // Speed at which the gun returns to its original position

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 currentRecoilPosition;
    private Quaternion currentRecoilRotation;
    private bool isRecoiling = false;

    void Start()
    {
        // Store the original position and rotation of the gun
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // If the player is shooting, apply recoil
        if (Input.GetButton("Fire1"))
        {
            StartRecoil();
        }

        // Interpolate between the current position/rotation and the target position/rotation
        if (isRecoiling)
        {
            currentRecoilPosition = Vector3.Lerp(currentRecoilPosition, originalPosition + recoilOffset, Time.deltaTime * recoilSpeed);
            currentRecoilRotation = Quaternion.Lerp(currentRecoilRotation, originalRotation * Quaternion.Euler(recoilRotation), Time.deltaTime * recoilSpeed);

            // Check if the gun has reached the recoil position
            if (Vector3.Distance(currentRecoilPosition, originalPosition + recoilOffset) < 0.001f)
            {
                isRecoiling = false;
            }
        }
        else
        {
            currentRecoilPosition = Vector3.Lerp(currentRecoilPosition, originalPosition, Time.deltaTime * returnSpeed);
            currentRecoilRotation = Quaternion.Lerp(currentRecoilRotation, originalRotation, Time.deltaTime * returnSpeed);
        }

        // Apply the interpolated position and rotation to the gun
        transform.localPosition = currentRecoilPosition;
        transform.localRotation = currentRecoilRotation;
    }

    void StartRecoil()
    {
        isRecoiling = true;
        currentRecoilPosition = transform.localPosition;
        currentRecoilRotation = transform.localRotation;
    }
}
