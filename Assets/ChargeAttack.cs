using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    BossAI bossAI;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        bossAI = GetComponent<BossAI>();
        
    }

    public void ChargeExecute()
    {
        anim.SetBool("charge", true);
        bossAI.canLookAt = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("charge"))
        {
            rb.velocity = transform.forward * 15;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("wall"))
        {
            anim.SetBool("charge", false);
            bossAI.canLookAt = true;
        }

    }
}
