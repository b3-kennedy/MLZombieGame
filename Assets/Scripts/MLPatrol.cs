using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;

[System.Serializable]
public class ZombiePatrolGroup
{
    public Transform patrolPoint;
    public GameObject[] zombies;
}

public class MLPatrol : Agent
{

    public static MLPatrol Instance;

    public List<ZombiePatrolGroup> groups;
    public LayerMask sphereLayer;
    public Transform player;

    public float randomPatrolTime;
    float randomPatrolTimer;

    public float patrolTime;
    float timer;

    Vector2 pos;
    float posTimer;
    bool NewPosSet;


    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < groups.Count; i++)
        {
            foreach (var zombie in groups[i].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[i].patrolPoint;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TakeAction()
    {
        RequestDecision();
    }

    public override void OnEpisodeBegin()
    {
        if(player != null)
        {
            //player.transform.position = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 200));
        }
        
        //TakeAction();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int posX = actions.DiscreteActions[0] * 8;
        int posY = actions.DiscreteActions[1] * 8;

        float combinedDistance = 0;

        pos = new Vector2(posX, posY);

        List<Vector3> points = new List<Vector3>();



        if(player.gameObject.activeSelf && !NewPosSet)
        {

            for (int i = 0; i < groups.Count; i++)
            {
                Vector3 point = new Vector3(pos.x, 0, pos.y) + Random.insideUnitSphere * 50;
                points.Add(new Vector3(point.x, 0, point.z));
            }

            for (int i = 0;i < groups.Count;i++)
            {
                groups[i].patrolPoint.position = points[i];
                //foreach (var zombie in groups[i].zombies)
                //{
                //    zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
                //}
                combinedDistance += Vector3.Distance(player.transform.position, groups[i].patrolPoint.position);
            }
            
            if(combinedDistance < 75 * 5)
            {
                //player.gameObject.SetActive(false);
                //Debug.Log("Player Targeted");

                StartCoroutine(SetNewPoint());
                

                AddReward(1f);
                NewPosSet = true;
                player.gameObject.SetActive(false);
                EndEpisode();
                
            }
            else
            {
                //Debug.Log("missed");
                AddReward(-0.5f);
                EndEpisode();
            }
            

        }
        else
        {
            RandomPatrol(pos);
        }
        
        


    }

    IEnumerator SetNewPoint()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            foreach (var zombie in groups[i].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
                Debug.Log("spawn");
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void TargetedPatrol()
    {

    }

    void RandomPatrol(Vector2 pos)
    {
        randomPatrolTimer += Time.deltaTime;
        if(randomPatrolTimer >= randomPatrolTime)
        {
            int randomNum = Random.Range(0, groups.Count);

            int randomX = Random.Range(0, 25) * 8;
            int randomY = Random.Range(0, 25) * 8;

            groups[randomNum].patrolPoint.position = new Vector3(randomX, 0, randomY);


            foreach (var zombie in groups[randomNum].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[randomNum].patrolPoint;
            }
            randomPatrolTimer = 0;
        }

        

        //AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = Random.Range(0, 24);
        discreteAction[1] = Random.Range(0, 24);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeAction();
        }

        //if (player.gameObject.activeSelf)
        //{
        //    timer = patrolTime;
            
        //}

        timer += Time.deltaTime;
        if (timer >= patrolTime)
        {
            TakeAction();
            timer = 0;
        }
    }

    private void Update()
    {
        if (NewPosSet)
        {
            posTimer += Time.deltaTime;
            if(posTimer > 5f)
            {
                NewPosSet = false;
                posTimer = 0;
            }
        }
    }
}
