using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterZombieAI : MonoBehaviour
{

    public Transform playerPos;
    public Transform home;
    NavMeshAgent agent;
    public float targetDecayTime;
    float decayTimer;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPos != null)
        {
            agent.SetDestination(playerPos.position);
            decayTimer += Time.deltaTime;
            if(decayTimer > targetDecayTime)
            {
                playerPos = null;
                decayTimer = 0;
                agent.SetDestination(home.position);
            }
        }
    }
}
