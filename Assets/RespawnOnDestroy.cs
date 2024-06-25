using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnDestroy : MonoBehaviour
{

    bool isQuitting;


    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            GameObject obj = Instantiate(gameObject, transform.position, transform.rotation);
            obj.SetActive(true);
        }
    }
}
