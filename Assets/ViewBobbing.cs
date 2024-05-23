using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{

    public float intensity;
    public float intensityX;
    public float speed;
    float normalIntensity;
    float normalIntensityX;

    [HideInInspector] public PositionFollower follower;
    Vector3 originalOffset;
    float sinTime;

    PlayerMove move;

    Vector3 originalPos;

    ADS ads;

    // Start is called before the first frame update
    void Start()
    {
        move = transform.parent.parent.parent.parent.parent.parent.GetComponent<PlayerMove>();
        normalIntensity = intensity;
        normalIntensityX = intensityX;
        follower = GetComponent<PositionFollower>();
        originalOffset = follower.offset;
        ads = GetComponent<ADS>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ads.isAiming)
        {
            intensity = intensity / 5;
        }


        Vector3 inputVec = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        speed = move.currentSpeed;

        if(inputVec.magnitude > 0)
        {
            intensity = Mathf.Lerp(intensity, normalIntensity, Time.deltaTime * 10);
            //intensityX = Mathf.Lerp(intensityX, normalIntensityX, Time.deltaTime * 10);
            sinTime += Time.deltaTime * speed;
        }
        else
        {
            intensity = Mathf.Lerp(intensity, 0, Time.deltaTime * 10);
            //intensityX = Mathf.Lerp(intensityX, 0, Time.deltaTime * 10);
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
