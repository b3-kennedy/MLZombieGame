using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    private void OnDestroy()
    {
        SpawnDummy.Instance.Spawn();
    }
}
