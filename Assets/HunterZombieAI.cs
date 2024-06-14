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
    Animator anim;
    public float runSpeed;
    public float walkSpeed;
    bool attacked = false;
    public float attackRate;
    float attackTimer;
    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacked)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackRate)
            {
                attacked = false;
                attackTimer = 0;
            }
        }

        if(playerPos != null)
        {
            agent.SetDestination(playerPos.position);
            agent.speed = runSpeed;
            anim.SetBool("player", true);
            decayTimer += Time.deltaTime;

            if(Vector3.Distance(playerPos.position, transform.position) < 1.3f && !attacked)
            {
                playerPos.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                attacked = true;
            }

            if(decayTimer > targetDecayTime)
            {
                playerPos = null;
                agent.speed = walkSpeed;
                decayTimer = 0;
                agent.SetDestination(home.position);
                anim.SetBool("player", false);
            }
        }
    }
}
