using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{

    public float intensity;
    public float intensityX;
    public float adsIntensity = 0.0005f;
    public float adsIntensityX = 0.2f;
    public float speed;
    float normalIntensity;
    float normalIntensityX;

    [HideInInspector] public PositionFollower follower;
    Vector3 originalOffset;
    float sinTime;

    RigidbodyMovement move;

    Vector3 originalPos;

    ADS ads;

    // Start is called before the first frame update
    void Start()
    {
        move = RigidbodyMovement.Instance;
        normalIntensity = intensity;
        normalIntensityX = intensityX;
        follower = GetComponent<PositionFollower>();
        originalOffset = follower.offset;
        ads = GetComponent<ADS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ads.isAiming)
        {
            intensityX = adsIntensityX;
            intensity = adsIntensity;
            
        }
        else
        {
            intensity = normalIntensity;
            intensityX = normalIntensityX;
        }


        Vector3 inputVec = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        speed = move.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 2;

        if(inputVec.magnitude > 0)
        {
            intensity = Mathf.Lerp(intensity, normalIntensity, Time.deltaTime * 10);
            //intensityX = Mathf.Lerp(intensityX, normalIntensityX, Time.deltaTime * 10);
            sinTime += Time.deltaTime * speed;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        }

        float sinY = Mathf.Abs(intensity * Mathf.Sin(sinTime));
        Vector3 sinX = follower.transform.right * intensity * Mathf.Cos(sinTime) * intensityX;

        follower.offset = new Vector3
        {
            x = originalOffset.x,
            y = originalOffset.y + sinY,
            z = originalOffset.z
        };

        follower.offset += sinX;
    }
}
