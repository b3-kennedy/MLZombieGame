using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompAttack : MonoBehaviour
{

    Animator anim;
    public GameObject stompAOE;
    BossAI bossAI;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bossAI = GetComponent<BossAI>();
    }


    public void StompExecute()
    {
        anim.SetBool("stomp", true);
        stompAOE.SetActive(true);
    }

    public void EnableHit()
    {
        if(Vector3.Distance(bossAI.target.position, bossAI.transform.position) < stompAOE.GetComponent<CircleDrawer>().radius)
        {
            if (bossAI.target.GetComponent<RigidbodyMovement>().IsGrounded())
            {
                Debug.Log("player hit");
            }
            
        }
    }

    public void EndStomp()
    {
        anim.SetBool("stomp", false);
        stompAOE.SetActive(false);
        bossAI.OnEndAttack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
