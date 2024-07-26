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

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bossAI = GetComponent<BossAI>();
        
    }

    public void ChargeExecute()
    {
        startWaitTimer = true;

        
    }

    // Update is called once per frame
    void Update()
    {

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
            Debug.Log(num);
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
