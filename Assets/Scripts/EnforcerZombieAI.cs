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
    [HideInInspector] public Transform player;
    Animator anim;
    public float walkSpeed;
    public float runSpeed;
    public float maxAudioTime;
    float audioTimer;
    Vector3 audioPos;
    ScoutZombieAudioManager audioManager;
    public Transform groundCheck;

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
        if (Physics.Raycast(groundCheck.position, -Vector3.up, out RaycastHit hit, 1f))
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
                LookAtPoint(audioPos);
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
        

    }

    void Patrol()
    {
        anim.SetBool("patrolling", true);
        agent.speed = walkSpeed;
        if (Vector3.Distance(transform.position, agent.destination) < 1f)
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
            if (MLPatrol2.Instance != null)
            {
                MLPatrol2.Instance.AddReward(1f);
            }
            if(player != null && player.GetComponent<PlayerHealth>())
            {
                player.GetComponent<PlayerHealth>().TakeDamage(10f);
            }
            
            gameObject.SetActive(false);
        }
    }

    public void HeardSound(Vector3 pos)
    {
        state = ZombiePatrolAI.ZombieState.HEARD_SOUND;
        Debug.Log(state);
        audioPos = pos;
    }

    void LookAtPoint(Vector3 pos)
    {
        anim.SetBool("patrolling", false);
        anim.SetBool("attacking", false);
        agent.speed = 0;
        Vector3 dir = pos - transform.position;
        Quaternion toRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), 2 * Time.deltaTime);
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
        player = playerObj;
    }
}
