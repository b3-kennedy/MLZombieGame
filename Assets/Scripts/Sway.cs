using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{

    public bool enableMouseSway = true;
    public bool enableMoveRot = true;
    public bool enableMoveOnMove = true;

   [Header("Settings")]
    public float swayAmount;
    public float maxSway;
    public float smoothing;
    public float rotSwayMultiplier;
    public float adsRotSwayMultiplier;
    public float moveMultiplier;
    float normalRotSway;

    Quaternion initialRot;
    Vector3 gunStartPos;
    Transform gun;

    ADS ads;


    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.localRotation;
        normalRotSway = rotSwayMultiplier;
        gun = transform.GetChild(0);
        gunStartPos = gun.localPosition;
        ads = GetComponent<ADS>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseSway();
        RotSway();
        MoveGun();
    }

    void MouseSway()
    {
        if (enableMouseSway && !ads.isAiming)
        {
            float mouseX = Input.GetAxis("Mouse X") * swayAmount;
            float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

            mouseX = Mathf.Clamp(mouseX, -maxSway, maxSway);
            mouseY = Mathf.Clamp(mouseY, -maxSway, maxSway);

            Quaternion targetRotX = Quaternion.AngleAxis(-mouseX, Vector3.up);
            Quaternion targetRotY = Quaternion.AngleAxis(mouseY, Vector3.right);
            Quaternion targetRot = initialRot * targetRotX * targetRotY;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * smoothing);
        }
        else if (ads.isAiming)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 100);
        }

    }

    void RotSway()
    {
        if (enableMoveRot)
        {
            if (ads.isAiming)
            {
                rotSwayMultiplier = adsRotSwayMultiplier;
            }
            else
            {
                rotSwayMultiplier = normalRotSway;
            }
            float moveX = Input.GetAxis("Horizontal");
            Quaternion newRot = new Quaternion(gun.localRotation.x, gun.localRotation.y, moveX * rotSwayMultiplier, gun.localRotation.w);
            gun.localRotation = Quaternion.Lerp(gun.localRotation, newRot, Time.deltaTime * 10);
        }

    }

    void MoveGun()
    {
        if (enableMoveOnMove && !ads.isAiming)
        {
            float moveZ = Input.GetAxisRaw("Vertical");

            if (moveZ != 0)
            {
                float zPos = gun.localPosition.z + (moveZ * moveMultiplier);
                zPos = Mathf.Clamp(zPos, -0.37f, -0.33f);
                Vector3 newPos = new Vector3(gun.localPosition.x, gun.localPosition.y, zPos);
                gun.localPosition = Vector3.Lerp(gun.localPosition, newPos, Time.deltaTime * 10);
            }
            else
            {
                gun.localPosition = Vector3.Lerp(gun.localPosition, gunStartPos, Time.deltaTime * 10);
            }
        }



    }

}