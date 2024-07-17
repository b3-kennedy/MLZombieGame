using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    public Transform target;
    ThrowAttack throwAttack;
    ChargeAttack chargeAttack;
    WaveAttack waveAttack;
    StompAttack stompAttack;
    float distance;
    [HideInInspector] public bool canLookAt;

    // Start is called before the first frame update
    void Start()
    {
        throwAttack = GetComponent<ThrowAttack>();
        chargeAttack = GetComponent<ChargeAttack>();
        waveAttack = GetComponent<WaveAttack>();
        stompAttack = GetComponent<StompAttack>();
        SelectState();
        
    }

    void SelectState()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if(distance < 10)
        {
            stompAttack.StompExecute();
        }
        else if(distance >= 10)
        {
            int num = Random.Range(0, 3);

            if(num == 0)
            {
                chargeAttack.ChargeExecute();
            }
            else if(num == 1)
            {
                waveAttack.WaveExecute();
            }
            else if(num == 2)
            {
                throwAttack.Execute();
            }
        }
    }

    public void OnEndAttack()
    {
        SelectState();
    }

    // Update is called once per frame
    void Update()
    {

        

        LookAtPoint(target.position);
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    chargeAttack.ChargeExecute();
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    waveAttack.StartAttack();
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    stompAttack.StompExecute();
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    throwAttack.Execute();
        //}
    }

    void LookAtPoint(Vector3 pos)
    {
        if (canLookAt)
        {
            Vector3 dir = pos - transform.position;
            Quaternion toRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), 2 * Time.deltaTime);
        }

    }
}
