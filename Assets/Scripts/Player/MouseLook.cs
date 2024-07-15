using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //public Transform cam;
    //public Transform orientation;
    //public float sensitivity;


    //float xRot;
    //float yRot;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
    //    float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

    //    yRot += mouseX;
    //    xRot -= mouseY;

    //    xRot = Mathf.Clamp(xRot, -90f, 90f);

    //    cam.localRotation = Quaternion.Euler(xRot, yRot, 0);
    //    orientation.rotation = Quaternion.Euler(0, yRot, 0);
    //}

    public Transform cam;
    Vector2 turnVec;
    public float sensitivity = 1.0f;
    public float maxSensitivity = 5.0f;
    public Transform camHolder;
    public Transform orientation;

    ADS adsScript;

    public bool acog;
    public bool holo;

    float acogSens;
    float holoSens;
    float normalSens;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (SettingsMenuManager.Instance != null)
        {
            SettingsMenuManager.Instance.updatedSens.AddListener(OnSensitivityChange);
            OnSensitivityChange();
        }
        normalSens = sensitivity;
    }

    public void OnGunPickedUp()
    {
        adsScript = GetComponent<PickUpWeapons>().currentActiveGun.transform.GetChild(0).GetComponent<ADS>();
        adsScript.aimedIn.AddListener(OnAds);
        adsScript.aimedOut.AddListener(OnUnAds);
    }

    public void OnSensitivityChange()
    {
        sensitivity = maxSensitivity * (SettingsMenuManager.Instance.mouseSensValue / 100);
        acogSens = sensitivity * SettingsMenuManager.Instance.acogSensValue;
        holoSens = sensitivity * SettingsMenuManager.Instance.holoSensValue;
        normalSens = sensitivity;
        
    }

    public void OnAds()
    {
        normalSens = sensitivity;
        if (acog)
        {
            sensitivity = acogSens;
        }
        else if (holo)
        {
            sensitivity = holoSens;
        }
        else
        {
            sensitivity = normalSens;
        }
    }

    public void OnUnAds()
    {
        sensitivity = normalSens;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance != null && GameManager.Instance.gameOver)
        {
            return;
        }


        turnVec.x += Input.GetAxisRaw("Mouse X") * sensitivity;
        turnVec.y += Input.GetAxisRaw("Mouse Y") * sensitivity;
        turnVec.y = Mathf.Clamp(turnVec.y, -90, 90);
        cam.localRotation = Quaternion.Euler(-turnVec.y, 0, 0);
        
        orientation.rotation = Quaternion.Euler(transform.localEulerAngles.x, turnVec.x, transform.localEulerAngles.z);
        camHolder.transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, turnVec.x, transform.localEulerAngles.z);
        
    }
}
