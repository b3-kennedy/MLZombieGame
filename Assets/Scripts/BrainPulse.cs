using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainPulse : MonoBehaviour
{

    public AnimationCurve pulse;
    public float pulseMultiplier = 1;
    float pulseTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pulseTimer += Time.deltaTime * pulseMultiplier;
        float pulseValue = pulse.Evaluate(pulseTimer);
        if(pulseTimer < 0.5f)
        {
            transform.localScale = new Vector3(transform.localScale.x + pulseValue, transform.localScale.y + pulseValue, transform.localScale.z + pulseValue);
        }
        else if (pulseTimer > 0.5f)
        {
            transform.localScale = new Vector3(transform.localScale.x - pulseValue, transform.localScale.y - pulseValue, transform.localScale.z - pulseValue);
        }
        

        if(pulseTimer >= 1)
        {
            pulseTimer = 0;
        }
    }
}
