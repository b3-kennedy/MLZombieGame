using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : MonoBehaviour
{
    public Animator anim;
    public Transform throwPoint;
    public GameObject rock;
    GameObject spawnedRock;
    BossAI boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<BossAI>();
    }

    public void Execute()
    {
        spawnedRock = Instantiate(rock, throwPoint.transform.position, Quaternion.identity);
        spawnedRock.transform.SetParent(throwPoint);
        spawnedRock.transform.localPosition = Vector3.zero;
        spawnedRock.GetComponent<Rigidbody>().isKinematic = true;
        spawnedRock.GetComponent<Collider>().enabled = false;
        anim.SetBool("throw", true);
    }

    public void Launch()
    {
        if(spawnedRock != null)
        {
            Vector3 dir = boss.target.position - transform.position;
            spawnedRock.transform.SetParent(null);
            spawnedRock.GetComponent<Rigidbody>().isKinematic = false;
            spawnedRock.GetComponent<Rigidbody>().AddForce(dir * 1, ForceMode.Impulse);
            spawnedRock.GetComponent<Collider>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
