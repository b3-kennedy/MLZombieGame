using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    Rigidbody rb;
    public Animator anim;
    BossAI bossAI;

    bool startWaitTimer;
    float waitTimer;
    bool distanceCheck;
    float distanceCheckTimer;
    public float damage;
    public float force;
    bool collided;
    public float damageCooldown;
    float damageTimer;
    public Transform groundCheck;
    ScoutZombieAudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bossAI = GetComponent<BossAI>();
        audioManager = GetComponent<ScoutZombieAudioManager>();
        
    }

    public void ChargeExecute()
    {
        if (GetComponent<StompAttack>().stompAOE.activeSelf)
        {
            GetComponent<StompAttack>().stompAOE.SetActive(false);
        }
        startWaitTimer = true;
        anim.SetBool("charge", true);
        anim.SetBool("throw", false);
        anim.SetBool("stomp", false);
        anim.SetBool("wave", false);

    }

    public void PlayFootseps()
    {

        GetSurface();
    }

    void GetSurface()
    {
        var surfaces = Physics.RaycastAll(groundCheck.position, -Vector3.up, 1f);


        foreach (var surface in surfaces)
        {
            if (surface.collider.GetComponent<Material>())
            {
                Material mat = surface.collider.GetComponent<Material>();
                switch (mat.matType)
                {
                    case Material.MaterialType.GRASS:
                        audioManager.PlayFootstep(audioManager.grassStepsWalk);
                        break;
                    case Material.MaterialType.CONCRETE:
                        audioManager.PlayFootstep(audioManager.concreteStepsWalk);
                        break;
                    case Material.MaterialType.WOOD:
                        audioManager.PlayFootstep(audioManager.woodStepsWalk);
                        break;

                    default:
                        audioManager.PlayFootstep(audioManager.grassStepsWalk);
                        break;
                }
            }
        }

        //if (Physics.Raycast(groundCheck.position, -Vector3.up, out RaycastHit hit, 1f))
        //{
        //    Debug.Log(hit.collider);   
        //    if (hit.collider.GetComponent<Material>())
        //    {
        //        Debug.Log("test");
        //        Material mat = hit.collider.GetComponent<Material>();
        //        switch (mat.matType)
        //        {
        //            case Material.MaterialType.GRASS:
        //                audioManager.PlayFootstep(audioManager.grassStepsWalk);
        //                break;
        //            case Material.MaterialType.CONCRETE:
        //                audioManager.PlayFootstep(audioManager.concreteStepsWalk);
        //                break;
        //            default:
        //                audioManager.PlayFootstep(audioManager.grassStepsWalk);
        //                break;
        //        }
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(groundCheck.position, -Vector3.up * 1f);
        Gizmos.DrawRay(ray);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(groundCheck.position, -Vector3.up * 1f, Color.white);
        if (collided)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer >= damageCooldown)
            {
                damageTimer = 0;
                collided = false;
            }
        }

        if (distanceCheck)
        {
            distanceCheckTimer += Time.deltaTime;
            if(distanceCheckTimer > 1)
            {
                distanceCheck = false;
                distanceCheckTimer = 0;
            }
        }

        if (startWaitTimer)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > 0.5f)
            {
                anim.SetBool("charge", true);
                anim.SetBool("throw", false);
                bossAI.canLookAt = false;
                waitTimer = 0;
                startWaitTimer = false;
            }
        }

        if (anim.GetBool("charge"))
        {
            rb.velocity = transform.forward * 15;
        }

        if(Vector3.Distance(transform.position, bossAI.target.position) <= 10 && !distanceCheck)
        {
            int num = Random.Range(0, 3);
            if(num == 0)
            {
                anim.SetBool("charge", false);
                bossAI.canLookAt = true;
                bossAI.OnEndAttack();
            }
            distanceCheck = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.collider.CompareTag("wall"))
        {
            anim.SetBool("charge", false);
            bossAI.canLookAt = true;
            bossAI.OnEndAttack();
        }
        else if (other.collider.GetComponent<PlayerHealth>() && !collided)
        {
            collided = true;
            bossAI.target.GetComponent<PlayerHealth>().TakeDamage(damage);
            Vector3 dir = (other.collider.transform.position - transform.position).normalized;
            other.collider.GetComponent<Rigidbody>().AddForce(dir * force);
        }

    }
}
