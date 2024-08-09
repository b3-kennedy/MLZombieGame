using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public Transform fallRespawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RigidbodyMovement>())
        {
            other.GetComponent<Rigidbody>().MovePosition(fallRespawnPos.position);
            other.transform.rotation = fallRespawnPos.rotation;
        }
    }
}
