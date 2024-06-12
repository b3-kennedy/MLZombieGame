using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMove : MonoBehaviour
{

    public Transform brainPos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = brainPos.position - transform.position;
        dir.Normalize();

        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void Update()
    {
        if(Vector3.Distance(brainPos.position, transform.position) < 1)
        {
            MLSpawn.Instance.End();
        }
    }
}
