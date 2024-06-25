using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;


public class MLPatrol2 : Agent
{

    public static MLPatrol2 Instance;
    public Transform[] playerSpawns;
    public List<ZombiePatrolGroup> groups;
    public LayerMask sphereLayer;
    public Transform player;
    PlayerHealth playerHealth;
    public float randomPatrolTime;
    float randomPatrolTimer;

    public float patrolTime;
    float timer;

    Vector2 pos;


    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < groups.Count; i++)
        {
            foreach (var zombie in groups[i].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[i].patrolPoint;
                Vector3 randomPos = Random.insideUnitSphere * 75;
                zombie.transform.position = groups[i].patrolPoint.position + new Vector3(randomPos.x,0,randomPos.z);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.parent.GetComponent<PlayerHealth>();
    }

    public void TakeAction()
    {
        RequestDecision();
    }

    public void ResetPosition()
    {
        int randomNum = Random.Range(0, playerSpawns.Length);
        playerHealth.currentHealth = 100;
        Debug.Log(randomNum);
        player.parent.GetComponent<NavMeshAgent>().Warp(playerSpawns[randomNum].position);
        player.parent.GetComponent<NavMeshAgent>().SetDestination(player.parent.GetComponent<PlayerAIMove>().target.position);
    }

    public override void OnEpisodeBegin()
    {
        if (player.parent != null)
        {
            //ResetPosition();
            
            player.gameObject.SetActive(false);
            ResetPosition();
            //player.parent.GetComponent<NavMeshAgent>().SetDestination(player.parent.GetComponent<PlayerAIMove>().target.position);
        }

        //TakeAction();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int posX = actions.DiscreteActions[0] * 8;
        int posY = actions.DiscreteActions[1] * 8;
        int groupNum = actions.DiscreteActions[2];
        int boolVal = actions.DiscreteActions[3];

        float combinedDistance = 0;

        pos = new Vector2(posX, posY);

        List<Vector3> points = new List<Vector3>();
        if(boolVal == 1)
        {
            RandomPatrol(pos, groupNum);
        }
        else
        {
            return;
        }
        

        //if (player.gameObject.activeSelf)
        //{

        //    for (int i = 0; i < groups.Count; i++)
        //    {
        //        Vector3 point = new Vector3(pos.x, 0, pos.y) + Random.insideUnitSphere * 50;
        //        points.Add(new Vector3(point.x, 0, point.z));
        //    }

        //    for (int i = 0; i < groups.Count; i++)
        //    {
        //        groups[i].patrolPoint.position = points[i];
        //        foreach (var zombie in groups[i].zombies)
        //        {
        //            zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
        //        }
        //        combinedDistance += Vector3.Distance(player.transform.position, groups[i].patrolPoint.position);
        //    }

        //    if (combinedDistance < 75 * 5)
        //    {
        //        //player.gameObject.SetActive(false);
        //        Debug.Log("Player Targeted");

        //        for (int i = 0; i < groups.Count; i++)
        //        {
        //            foreach (var zombie in groups[i].zombies)
        //            {
        //                zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
        //            }
        //        }
        //        player.gameObject.SetActive(false);

        //        AddReward(1f);
        //        EndEpisode();
        //    }
        //    else
        //    {
        //        Debug.Log("missed");
        //        AddReward(-0.5f);
        //        EndEpisode();
        //    }


        //}




    }

    void TargetedPatrol()
    {

    }

    void RandomPatrol(Vector2 pos, int group)
    {

        groups[group].patrolPoint.position = new Vector3(pos.x, 0, pos.y);


        foreach (var zombie in groups[group].zombies)
        {
            zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[group].patrolPoint;
            zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
        }



        //AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = Random.Range(0, 24);
        discreteAction[1] = Random.Range(0, 24);
        discreteAction[2] = Random.Range(0, 10);
        discreteAction[3] = Random.Range(0, 2);

    }

    public void GainReward(float points)
    {
        AddReward(points);
    }

    public void End()
    {
        Debug.Log("end");
        //ResetPosition();

        EndEpisode();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomPatrolTime)
        {
            TakeAction();
            timer = 0;
        }
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
    }
}
