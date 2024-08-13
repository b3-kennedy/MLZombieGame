using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
    bool patrol;
    float patrolTimer;
    public float patrolRadius;


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
        if (audioManager.footstepSource.enabled)
        {

            RaycastHit[] results = new RaycastHit[3];
            Physics.RaycastNonAlloc(groundCheck.position, -Vector3.up, results, 1f);

            foreach (var hit in results)
            {
                if (hit.collider != null)
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

        if(playerPos != null && !patrol)
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
                
                audioManager.footstepSource.volume = 0.5f;
                patrol = true;
            }
        }
        else if (patrol)
        {
            agent.speed = walkSpeed;
            anim.SetBool("player", false);
            patrolTimer += Time.deltaTime;
            if(patrolTimer >= 5)
            {
                GenerateNewPoint();
                patrolTimer = 0;
            }
        }

    }

    public void DoorUnlocking()
    {
        patrol = false;
        playerPos = GameManager.Instance.player.transform;
    }

    public void GenerateNewPoint()
    {
        if (!GetComponent<Health>().dead)
        {
            Vector3 point = home.position + (Random.insideUnitSphere * patrolRadius);
            Vector3 newDestPoint = new Vector3(point.x, 0, point.z);
            if (gameObject.activeSelf)
            {
                NavMeshPath newPath = new NavMeshPath();
                if (agent.CalculatePath(newDestPoint, newPath) && newPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(newDestPoint);
                }
                else
                {
                    agent.SetDestination(transform.position);
                }

            }
        }


    }

    public void DealDamage()
    {
        if(playerPos != null)
        {
            if (Vector3.Distance(playerPos.position, transform.position) < 1.3f)
            {

                playerPos.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }

        
    }
}
