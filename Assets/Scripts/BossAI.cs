using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossState {CHARGE, WAVE, THROW, STOMP};
    public BossState state;
    public Transform target;
    ThrowAttack throwAttack;
    ChargeAttack chargeAttack;
    WaveAttack waveAttack;
    StompAttack stompAttack;
    float distance;
    [HideInInspector] public bool canLookAt;
    float betweenStateTimer;
    bool startbetweenStateTimer;

    float waveTimer;
    bool startWaveCd;

    // Start is called before the first frame update
    void Start()
    {
        throwAttack = GetComponent<ThrowAttack>();
        chargeAttack = GetComponent<ChargeAttack>();
        waveAttack = GetComponent<WaveAttack>();
        stompAttack = GetComponent<StompAttack>();
        
        
    }

    void SelectState()
    {
        if (waveAttack.drawer.gameObject.activeSelf)
        {
            waveAttack.drawer.gameObject.SetActive(false);
        }
        distance = Vector3.Distance(transform.position, target.position);
        if(distance < 10)
        {
            int num = Random.Range(0, 2);
            if(num == 0 && state != BossState.STOMP)
            {
                state = BossState.STOMP;
                stompAttack.StompExecute();
            }
            else if(num == 1 && state != BossState.CHARGE)
            {
                state = BossState.CHARGE;
                chargeAttack.ChargeExecute();
            }
            else
            {
                SelectState();
            }
            
        }
        else if(distance >= 10)
        {
            int num = Random.Range(0, 3);

            if(num == 0 && state != BossState.CHARGE)
            {
                state = BossState.CHARGE;
                chargeAttack.ChargeExecute();
            }
            else if(num == 1 && state != BossState.WAVE && !startWaveCd)
            {
                state = BossState.WAVE;
                waveAttack.StartAttack();
                startWaveCd = true;
            }
            else if(num == 2 && state != BossState.THROW)
            {
                state = BossState.THROW;
                throwAttack.Execute();
            }
            else
            {
                SelectState();
            }
        }
    }

    public void StartBossFight()
    {
        SelectState();
    }

    public void OnEndAttack(string script)
    {
        startbetweenStateTimer = true;
        Debug.Log(script);
    }

    // Update is called once per frame
    void Update()
    {

        if (state != BossState.CHARGE)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
                
        }

        if (state == BossState.WAVE)
        {
            waveAttack.WaveUpdate();
        }

        if (startWaveCd)
        {
            waveTimer += Time.deltaTime;
            if(waveTimer > 5f)
            {
                waveTimer = 0;
                startWaveCd = false;
            }
        }

        if (startbetweenStateTimer)
        {
            betweenStateTimer += Time.deltaTime;
            if(betweenStateTimer > 0.5f)
            {
                SelectState();
                betweenStateTimer = 0;
                startbetweenStateTimer = false;
            }
        }

        LookAtPoint(target.position);
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartBossFight();
        }
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
            transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), 5 * Time.deltaTime);
        }

    }
}
