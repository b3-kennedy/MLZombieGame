using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ADS : MonoBehaviour
{

    Transform gun;
    public Vector3 normalPosition;
    public Vector3 adsPosition;     
    public float adsSpeed = 5.0f;     

    [HideInInspector] public bool isAiming = false;   
    private float aimProgress = 0.0f;


    private void Start()
    {
        gun = transform.GetChild(0);
        //normalPosition = gun.localPosition;
    }

    //Vector3(0,-0.0680000037,-0.184)

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  
        {
            transform.localPosition = Vector3.zero;
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        if (isAiming)
        {
            aimProgress += Time.deltaTime * adsSpeed;
        }
        else
        {
            aimProgress -= Time.deltaTime * adsSpeed;
        }

        aimProgress = Mathf.Clamp01(aimProgress);

        gun.transform.localPosition = Vector3.Lerp(normalPosition, adsPosition, aimProgress);
    }
}

