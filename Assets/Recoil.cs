using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 currentRot;
    Vector3 targetRot;

    [HideInInspector] public float recoilX;
    [HideInInspector] public float recoilY;
    [HideInInspector] public float recoilZ;

    [HideInInspector] public float snap;
    [HideInInspector] public float returnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, snap * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRot);
    }

    public void RecoilFire()
    {
        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
