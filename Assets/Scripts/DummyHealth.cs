using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealth : Health
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float dmg)
    {
        if (health - dmg <= 0)
        {
            transform.position = new Vector3(Random.Range(SpawnDummy.Instance.topLeft.position.x, SpawnDummy.Instance.bottomLeft.position.x), 0, Random.Range(SpawnDummy.Instance.topLeft.position.z, SpawnDummy.Instance.topRight.position.z));
            health = 100;
        }
        else
        {
            health -= dmg;
        }
    }
}
