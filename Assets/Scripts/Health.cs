using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public Animator anim;
    public List<Collider> colliders;
    [HideInInspector] public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            dead = true;
            if(anim != null)
            {
                anim.SetBool("dead", true);
                GetComponent<NavMeshAgent>().ResetPath();
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                if (GetComponent<ZombiePatrolAI>())
                {
                    GetComponent<ZombiePatrolAI>().enabled = false;
                    GetComponent<AISensor>().enabled = false;
                }
                else if (GetComponent<EnforcerZombieAI>())
                {
                    GetComponent<EnforcerZombieAI>().enabled = false;
                    GetComponent<AISensor>().enabled = false;
                }
                else if (GetComponent<HunterZombieAI>())
                {
                    GetComponent<HunterZombieAI>().enabled = false;
                }

                foreach(Collider col in colliders) 
                {
                    col.enabled = false;
                }
                
            }
            else
            {
                gameObject.SetActive(false);
            }
            
            //gameObject.SetActive(false);
        }
    }
}
