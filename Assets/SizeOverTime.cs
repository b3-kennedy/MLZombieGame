using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOverTime : MonoBehaviour
{

    public AnimationCurve size;
    public float timeMultiplier;
    float value;
    public float maxSize = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(value < maxSize)
        {
            value += size.Evaluate(Time.deltaTime * timeMultiplier);
            transform.localScale = new Vector3(value, value, value);
        }

    }
}
