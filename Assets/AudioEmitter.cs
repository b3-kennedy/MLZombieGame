using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    public float range;
    public LayerMask layers;


    public void Alert()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range, layers);
        foreach (Collider col in cols)
        {
            if (col.GetComponent<ZombiePatrolAI>())
            {
                col.GetComponent<ZombiePatrolAI>().HeardSound(transform.position);
            }
            
        }
    }
}
