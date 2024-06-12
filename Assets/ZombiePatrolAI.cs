using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolAI : MonoBehaviour
{

    public Transform patrolPoint;
    public float radius;
    public float positionChangeInterval;
    float timer;
    NavMeshAgent agent;
    Vector3 destPoint;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        GenerateNewPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, destPoint) < 1f)
        {
            anim.SetBool("moving", false);
            timer += Time.deltaTime;
            if (timer > positionChangeInterval)
            {
                GenerateNewPoint();
                timer = 0;
            }
        }
        else
        {
            anim.SetBool("moving", true);
        }

    }

    public void GenerateNewPoint()
    {
        Vector3 point = patrolPoint.position + (Random.insideUnitSphere * radius);
        destPoint = new Vector3(point.x, 0, point.z);
        agent.SetDestination(destPoint);
    }

    public void AlertBrain(Transform pos)
    {
        //TestZombieBrain2.Instance.Hunt(pos);
        //MLPatrol.Instance.TakeAction();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPoint.position, radius);
        Gizmos.color = Color.green;
    }
}
