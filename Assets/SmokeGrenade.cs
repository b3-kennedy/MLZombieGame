using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    public float fuseTime = 1.0f;
    float fuseTimer;
    bool startFuse;
    [HideInInspector] public bool isActive;
    public GameObject smokeGrenadeSmoke;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startFuse)
        {
            fuseTimer += Time.deltaTime;
            if(fuseTimer >= fuseTime)
            {
                EmitSmoke();
                //fuseTimer = 0;

            }
        }
    }

    void EmitSmoke()
    {
        Instantiate(smokeGrenadeSmoke, transform.position, Quaternion.identity);

        isActive = false;
        startFuse = false;
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            Debug.Log("test");
            startFuse = true;
        }

    }
}
