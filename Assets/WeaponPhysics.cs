using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysics : MonoBehaviour
{

    public float rotSwayMultiplier;

    //private void Update()
    //{
    //    float moveX = Input.GetAxis("Horizontal");
    //    Quaternion newRot = new Quaternion(transform.localRotation.x, transform.localRotation.y, moveX * rotSwayMultiplier, transform.localRotation.w);
    //    transform.localRotation = Quaternion.Lerp(transform.localRotation, newRot, Time.deltaTime * 10);
    //}
}
