using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnCollision : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] public bool active;

    float timer;


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1f)
        {
            active = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (active)
        {
            Debug.Log("break");
        }
        
    }
}
