using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        Vector3 inputVec = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        if(inputVec.magnitude > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * 10);
        }
        

    }
}
