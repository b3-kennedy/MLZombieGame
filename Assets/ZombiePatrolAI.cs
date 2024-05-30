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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GenerateNewPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, destPoint) < 1f)
        {
            timer += Time.deltaTime;
            if (timer > positionChangeInterval)
            {
                GenerateNewPoint();
                timer = 0;
            }
        }

    }

    void GenerateNewPoint()
    {
        Vector3 point = patrolPoint.position + (Random.insideUnitSphere * radius);
        destPoint = new Vector3(point.x, 0, point.z);
        agent.SetDestination(destPoint);
    }

    public void AlertBrain(Transform pos)
    {
        TestZombieBrain2.Instance.Hunt(pos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPoint.position, radius);
        Gizmos.color = Color.green;
    }
}
