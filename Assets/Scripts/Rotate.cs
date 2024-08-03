using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float rotateSpeed;

    public Vector3 rotateVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateVec.x * rotateSpeed * Time.deltaTime, rotateVec.y * rotateSpeed * Time.deltaTime, rotateVec.z * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
