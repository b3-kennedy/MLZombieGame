using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    public float normalSpeed;
    public float sprintSpeed;
    public AudioSource alertedSource;
    bool playAlertSound = true;
    float alertSoundTimer;
    ScoutZombieAudioManager szam;
    [Header("Audio Settings")]
    public float walkingVolume = 0.25f;
    public float runningVolume = 0.5f;
    public Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        szam = GetComponent<ScoutZombieAudioManager>();
        spotTimer = spotCd;
        GenerateNewPoint();
    }

    public void PlayFootsteps()
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
                        szam.PlayFootstep(szam.grassStepsWalk);
                        break;
                    case Material.MaterialType.CONCRETE:
                        szam.PlayFootstep(szam.concreteStepsWalk);
                        break;
                    default:
                        szam.PlayFootstep(szam.grassStepsWalk);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.velocity != Vector3.zero && !anim.GetBool("run"))
        {
            anim.SetBool("moving", true);
        }

        if(state == ZombieState.NORMAL)
        {
            if (!playerSpotted)
            {
                //if (Vector3.Distance(transform.position, destPoint) < 25f && Vector3.Distance(transform.position, destPoint) >= 0.5f)
                //{
                //    agent.speed = normalSpeed;
                //    anim.SetBool("run", false);
                //    anim.SetBool("moving", true);
                //    Debug.Log("move");
                //}
                if (Vector3.Distance(transform.position, agent.destination) < 0.2f)
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
                else if (Vector3.Distance(transform.position, agent.destination) > 100f)
                {
                    agent.speed = sprintSpeed;
                    anim.SetBool("moving", false);
                    anim.SetBool("run", true);
                    szam.footstepSource.volume = runningVolume;
                }
                else if(Vector3.Distance(transform.position, agent.destination) > 0.3f && Vector3.Distance(transform.position, destPoint) < 100f)
                {
                    agent.speed = normalSpeed;
                    anim.SetBool("run", false);
                    anim.SetBool("moving", true);
                    szam.footstepSource.volume = walkingVolume;
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

        spotTimer += Time.deltaTime;
        if (spotTimer >= spotCd)
        {
            canSpawn = true;
            spotTimer = 0;
        }

        if (!playAlertSound)
        {
            alertSoundTimer += Time.deltaTime;
            if(alertSoundTimer >= 5f)
            {
                playAlertSound = true;
                alertSoundTimer = 0;
                
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

    public void HeardSound(Vector3 pos)
    {
        state = ZombieState.HEARD_SOUND;
        audioPos = pos;
        audioHeard = true;
    }

    public void AlertBrain(Transform pos)
    {
        playerSpotted = true;
        playerPos = pos;


        //TestZombieBrain2.Instance.Hunt(pos);
        //MLPatrol.Instance.TakeAction();

        
        if (playAlertSound)
        {
            alertedSource.pitch = Random.Range(0.5f, 1f);
            alertedSource.Play();
            playAlertSound = false;
        }


        if (canSpawn)
        {

            //MLSpawn.Instance.SpawnHunter();
            if (MLPatrol2.Instance != null)
            {
                MLPatrol2.Instance.GainReward(1f);
                MLPatrol2.Instance.RequestAction();
            }

            if(GameManager.Instance != null)
            {
                GameManager.Instance.player.GetComponent<PlayerHealth>().TakeDamage(2f);
            }
            
            canSpawn = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPoint.position, radius);
        Gizmos.color = Color.green;
    }
}