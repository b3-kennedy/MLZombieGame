using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnforcerZombieAI : MonoBehaviour
{
    public Transform patrolPoint;
    public float radius;
    public float positionChangeInterval;
    float timer;
    NavMeshAgent agent;
    Vector3 destPoint;
    Transform player;
    Animator anim;
    public float walkSpeed;
    public float runSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        GenerateNewPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Patrol();
        }
        else
        {
            Attack();
        }
        

    }

    void Patrol()
    {
        anim.SetBool("patrolling", true);
        agent.speed = walkSpeed;
        if (Vector3.Distance(transform.position, destPoint) < 1f)
        {
            
            anim.SetBool("patrolling", false);
            timer += Time.deltaTime;
            if (timer > positionChangeInterval)
            {
                
                GenerateNewPoint();
                timer = 0;
            }
        }
    }

    void Attack()
    {
        agent.speed = runSpeed;
        anim.SetBool("patrolling", false);
        anim.SetBool("attacking", true);
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) < 1.5f)
        {
            Debug.Log("attack");
        }
    }

    void GenerateNewPoint()
    {
        Vector3 point = patrolPoint.position + (Random.insideUnitSphere * radius);
        destPoint = new Vector3(point.x, 0, point.z);
        agent.SetDestination(destPoint);
    }

    public void SpottedPlayer(Transform playerObj)
    {
        player = playerObj;
    }
}
