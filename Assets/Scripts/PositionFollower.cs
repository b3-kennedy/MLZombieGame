using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{

    [HideInInspector] public Transform targetTransform;
    public Vector3 offset;


    private void Start()
    {
        targetTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * 100);
    }
}
