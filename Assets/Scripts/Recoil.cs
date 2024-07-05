using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    public static Recoil Instance;

    public Vector3 currentRot;
    public Vector3 targetRot;

    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [HideInInspector] public float snap;
    [HideInInspector] public float returnSpeed;



    private void Awake()
    {
        Instance = this;
    }

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
