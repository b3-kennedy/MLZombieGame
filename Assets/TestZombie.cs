using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZombie : MonoBehaviour
{
    public TestZombieBrain zombieBrain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target"))
        {
            Debug.Log("targetHit");
            zombieBrain.AddReward(15);
            zombieBrain.End();
        }
    }
}
