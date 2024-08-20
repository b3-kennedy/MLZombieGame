using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnforcerZombieAI : MonoBehaviour
{

    public ZombiePatrolAI.ZombieState state;
    public Transform patrolPoint;
    public float radius;
    public float positionChangeInterval;
    float timer;
    NavMeshAgent agent;
    public Vector3 destPoint;
    public Transform player;
    Animator anim;
    public float walkSpeed;
    public float runSpeed;
    public float maxAudioTime;
    float audioTimer;
    [HideInInspector] public Vector3 audioPos;
    ScoutZombieAudioManager audioManager;
    public Transform groundCheck;
    bool attacked;
    public float attackRate;
    float attackTimer;
    public bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioManager = GetComponent<ScoutZombieAudioManager>();
        GenerateNewPoint();
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


    private void OnDrawGizmos()
    {
        Ray ray = new Ray(groundCheck.position, -Vector3.up * 0.03f);
        Gizmos.DrawRay(ray);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if(state == ZombiePatrolAI.ZombieState.NORMAL)
            {
                Patrol();
            }
            else if (state == ZombiePatrolAI.ZombieState.HEARD_SOUND)
            {
                LookAtPoint(audioPos, 2);
                audioTimer += Time.deltaTime;
                if(audioTimer >= maxAudioTime)
                {
                    audioTimer = 0;
                    state = ZombiePatrolAI.ZombieState.NORMAL;
                }
            }
            
        }
        else
        {
            Attack();
        }

        if (!canAttack)
        {
            //anim.SetBool("swipe", false);
            attackTimer += Time.deltaTime;
            {
                canAttack = true;
                
                attackTimer = 0;
            }
        }

    }

    public void DealDamage()
    {
        if (player.GetComponent<RigidbodyMovement>())
        {
            if(Vector3.Distance(transform.position, player.position) <= 2f)
            {
                Debug.Log("enforcer hit");
                player.GetComponent<PlayerHealth>().TakeDamage(50f);
            }

        }
        
    }

    void Patrol()
    {
        if(Vector3.Distance(transform.position, agent.destination) > 25f)
        {
            anim.SetBool("run", true);
            anim.SetBool("patrolling", false);
            agent.speed = runSpeed;
        }
        else if(Vector3.Distance(transform.position, agent.destination) > 1f && Vector3.Distance(transform.position, agent.destination) <= 25f)
        {
            anim.SetBool("patrolling", true);
            anim.SetBool("run", false);
            agent.speed = walkSpeed;
        }

        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            anim.SetBool("run", false);
            anim.SetBool("patrolling", false);
            timer += Time.deltaTime;
            if (timer > positionChangeInterval)
            {
                
                GenerateNewPoint();
                timer = 0;
            }
        }
    }


    public void CancelAttack()
    {
        anim.SetBool("swipe", false);
        agent.angularSpeed = 120;
    }

    void Attack()
    {
        agent.speed = runSpeed;
        anim.SetBool("attacking", true);
        anim.SetBool("patrolling", false);
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) < 2f)
        {
            if (canAttack)
            {
                agent.angularSpeed = 999;
                anim.SetBool("swipe", true);
                canAttack = false;
            }
            
            if (MLPatrol2.Instance != null)
            {
                MLPatrol2.Instance.AddReward(1f);
            }
            
            //gameObject.SetActive(false);
        }
    }

    public void HeardSound(Vector3 pos)
    {
        state = ZombiePatrolAI.ZombieState.HEARD_SOUND;
        audioPos = pos;
    }

    void LookAtPoint(Vector3 pos, float speed)
    {
        anim.SetBool("patrolling", false);
        anim.SetBool("attacking", false);
        agent.speed = 0;
        Vector3 dir = pos - transform.position;
        Quaternion toRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), speed * Time.deltaTime);
    }

    public void GenerateNewPoint()
    {
        Vector3 point = patrolPoint.position + (Random.insideUnitSphere * radius);
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

    public void SpottedPlayer(Transform playerObj)
    {
        if(player == null)
        {

            if (playerObj.GetComponent<RigidbodyMovement>())
            {
                    player = playerObj;
            }
            
            if(EnforcerBrain.Instance != null)
            {
                EnforcerBrain.Instance.AddReward(2f);
            }
            
            
        }

    }
}
