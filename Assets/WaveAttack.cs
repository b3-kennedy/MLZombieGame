using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    public Animator anim;
    public CircleDrawer drawer;
    public float multiplier;
    bool isActive;
    BossAI bossAI;
    bool canDamage;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        bossAI = GetComponent<BossAI>();
    }

    public void StartAttack()
    {
        anim.SetBool("wave", true);
    }

    public void WaveExecute()
    {
        canDamage = true;
        drawer.gameObject.SetActive(true);
        isActive = true;
    }


    public void EndAttack()
    {
        anim.SetBool("wave", false);
        drawer.gameObject.SetActive(false);
        drawer.radius = 1;
        isActive = false;
        bossAI.OnEndAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            drawer.radius += Time.deltaTime * multiplier;
        }


        if (Vector3.Distance(bossAI.target.position, bossAI.transform.position) < drawer.radius)
        {
            if (canDamage && !bossAI.target.GetComponent<RigidbodyMovement>().isCrouched)
            {
                bossAI.target.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

        }
        if (drawer.radius > Vector3.Distance(bossAI.target.position, bossAI.transform.position))
        {
            canDamage = false;
        }


    }
}
