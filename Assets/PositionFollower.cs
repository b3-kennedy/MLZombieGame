using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{

    public Transform targetTransform;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * 100);
    }
}
