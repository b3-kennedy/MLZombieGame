using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class ZombiePatrolAI : MonoBehaviour
{
    public enum ZombieState {NORMAL, HEARD_SOUND}
    public ZombieState state;
    public Transform patrolPoint;
    public float radius;
    public float positionChangeInterval;
    float timer;
    NavMeshAgent agent;
    Vector3 destPoint;
    Animator anim;
    bool canSpawn;
    public float spotCd;
    float spotTimer;
    [HideInInspector] public bool playerSpotted = false;
    Transform playerPos;
    bool audioHeard;
    Vector3 audioPos;
    public float maxAudioTime;
    float audioTimer;

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
        if(state == ZombieState.NORMAL)
        {
            if (!playerSpotted)
            {
                if (Vector3.Distance(transform.position, destPoint) < 1f)
                {
                    anim.SetBool("moving", false);
                    anim.SetBool("run", false);
                    timer += Time.deltaTime;
                    if (timer > positionChangeInterval)
                    {
                        GenerateNewPoint();
                        timer = 0;
                    }
                }
                else if (Vector3.Distance(transform.position, destPoint) > 100f)
                {
                    agent.speed = 5;
                    anim.SetBool("moving", false);
                    anim.SetBool("run", true);
                }
                else if (Vector3.Distance(transform.position, destPoint) < 25f)
                {
                    agent.speed = 1;
                    anim.SetBool("run", false);
                    anim.SetBool("moving", true);
                }

                spotTimer += Time.deltaTime;
                if (spotTimer >= spotCd)
                {
                    canSpawn = true;
                    spotTimer = 0;
                }
            }
            else
            {
                LookAtPoint(playerPos.position);
            }
        }
        else if(state == ZombieState.HEARD_SOUND)
        {
            LookAtPoint(audioPos);
            if (playerSpotted)
            {
                state = ZombieState.NORMAL;
            }
            else
            {
                audioTimer += Time.deltaTime;
                if(audioTimer >= maxAudioTime)
                {
                    state = ZombieState.NORMAL;
                    audioTimer = 0;
                }
            }
        }



    }

    void LookAtPoint(Vector3 pos)
    {
        anim.SetBool("run", false);
        anim.SetBool("moving", false);
        agent.speed = 0;
        Vector3 dir = pos - transform.position;
        Quaternion toRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), 2 * Time.deltaTime);
    }

    public void GenerateNewPoint()
    {
        Vector3 point = patrolPoint.position + (Random.insideUnitSphere * radius);
        destPoint = new Vector3(point.x, 0, point.z);
        if (gameObject.activeSelf)
        {
            NavMeshPath newPath = new NavMeshPath();
            if(agent.CalculatePath(destPoint, newPath) && newPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(destPoint);
            }
            else
            {
                GenerateNewPoint();
            }
            
        }
        
    }

    public void HeardSound(Vector3 pos)
    {
        state = ZombieState.HEARD_SOUND;
        audioPos = pos;
    }

    public void AlertBrain(Transform pos)
    {
        playerSpotted = true;
        playerPos = pos;
        //TestZombieBrain2.Instance.Hunt(pos);
        //MLPatrol.Instance.TakeAction();
        if (canSpawn)
        {
            MLSpawn.Instance.SpawnHunter();
            canSpawn = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPoint.position, radius);
        Gizmos.color = Color.green;
    }
}
