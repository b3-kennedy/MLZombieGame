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
    ScoutZombieAudioManager audioManager;
    public Transform groundCheck;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioManager = GetComponent<ScoutZombieAudioManager>();
    }


    public void PlayFootseps()
    {
        GetSurface();
    }

    void GetSurface()
    {
        if (Physics.Raycast(groundCheck.position, -Vector3.up, out RaycastHit hit, 0.03f))
        {
            if (hit.collider.GetComponent<Material>())
            {
                Material mat = hit.collider.GetComponent<Material>();
                switch (mat.matType)
                {
                    case Material.MaterialType.GRASS:
                        audioManager.PlayFootstep(audioManager.grassStepsWalk);
                        break;
                    case Material.MaterialType.CONCRETE:
                        audioManager.PlayFootstep(audioManager.concreteStepsWalk);
                        break;
                    default:
                        audioManager.PlayFootstep(audioManager.grassStepsWalk);
                        break;
                }
            }
        }
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
            audioManager.footstepSource.volume = 1f;
            if(Vector3.Distance(playerPos.position, transform.position) < 1.3f && !attacked)
            {
                
                anim.SetBool("attack", true);
                attacked = true;
            }
            else
            {
                anim.SetBool("attack", false);
            }

            if(decayTimer > targetDecayTime)
            {
                playerPos = null;
                agent.speed = walkSpeed;
                decayTimer = 0;
                agent.SetDestination(home.position);
                anim.SetBool("player", false);
                audioManager.footstepSource.volume = 0.5f;
            }
        }
    }

    public void DealDamage()
    {
        if (Vector3.Distance(playerPos.position, transform.position) < 1.3f)
        {

            playerPos.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        
    }
}
