using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAIMove : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    public void RandomSpawn()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < 5f)
        {
            agent.ResetPath();
            MLPatrol2.Instance.AddReward(-15f);
            MLPatrol2.Instance.End();
        }
    }
}
