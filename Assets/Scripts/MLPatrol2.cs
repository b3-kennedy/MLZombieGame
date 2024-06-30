using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;


[System.Serializable]
public class EnforcerPatrol
{
    public Transform patrolPoint;
    public GameObject enforcerZombie;
}

public class MLPatrol2 : Agent
{

    public static MLPatrol2 Instance;
    public Transform[] playerSpawns;
    public List<ZombiePatrolGroup> groups;
    public List<EnforcerPatrol> enforcerZombies;
    public LayerMask sphereLayer;
    public Transform player;
    PlayerHealth playerHealth;
    public float randomPatrolTime;
    float randomPatrolTimer;

    public float patrolTime;
    float timer;

    Vector2 pos;

    public List<Vector3> scoutZombieStartingPatrolPos;
    public List<Vector3> enforcerZombieStartingPatrolPos;


    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < groups.Count; i++)
        {
            foreach (var zombie in groups[i].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[i].patrolPoint;
                Vector3 randomPos = Random.insideUnitSphere * 75;
                zombie.transform.position = groups[i].patrolPoint.position + new Vector3(randomPos.x, 0, randomPos.z);
            }
        }
        //ResetZombiePositions();



        //for (int i = 0; i < enforcerZombies.Count; i++)
        //{
        //    enforcerZombies[i].enforcerZombie.GetComponent<EnforcerZombieAI>().patrolPoint = enforcerZombies[i].patrolPoint;
        //}
    }

    void ResetZombiePositions()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].patrolPoint.position = scoutZombieStartingPatrolPos[i];
            foreach (var zombie in groups[i].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[i].patrolPoint;
                Vector3 randomPos = Random.insideUnitSphere * 75;
                zombie.transform.position = groups[i].patrolPoint.position + new Vector3(randomPos.x, 0, randomPos.z);
                zombie.GetComponent<NavMeshAgent>().ResetPath();
            }
            
        }

        for (int i = 0;i < enforcerZombies.Count; i++)
        {
            enforcerZombies[i].enforcerZombie.SetActive(true);
            enforcerZombies[i].patrolPoint.position = enforcerZombieStartingPatrolPos[i];
            enforcerZombies[i].enforcerZombie.transform.position = enforcerZombieStartingPatrolPos[i];
            enforcerZombies[i].enforcerZombie.GetComponent<NavMeshAgent>().ResetPath();
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.parent.GetComponent<PlayerHealth>();
        foreach (var patrol in groups)
        {
            scoutZombieStartingPatrolPos.Add(patrol.patrolPoint.position);
        }

        foreach (var enforcer in enforcerZombies)
        {
            enforcerZombieStartingPatrolPos.Add(enforcer.patrolPoint.position);
        }

        ResetZombiePositions();
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

        ResetZombiePositions();

        player.parent.GetComponent<PlayerAIMove>().index = Random.Range(0, player.parent.GetComponent<PlayerAIMove>().positions.Count);
        player.parent.GetComponent<NavMeshAgent>().Warp(playerSpawns[randomNum].position);
        
        player.parent.GetComponent<PlayerAIMove>().OnReset();
        player.parent.GetComponent<PlayerAIMove>().PickPos();
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
        int enforcerNum = actions.DiscreteActions[4];
        int enforcerBoolVal = actions.DiscreteActions[5];


        pos = new Vector2(posX, posY);

        List<Vector3> points = new List<Vector3>();

        if(enforcerBoolVal == 1)
        {
            RandomEnforcerPatrol(pos, enforcerNum);
        }

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

    void RandomEnforcerPatrol(Vector2 pos, int group)
    {
        enforcerZombies[group].patrolPoint.position = new Vector3(pos.x, 0, pos.y);


        for (int i = 0; i < enforcerZombies.Count; i++)
        {
            enforcerZombies[i].enforcerZombie.GetComponent<EnforcerZombieAI>().patrolPoint = enforcerZombies[group].patrolPoint;
            enforcerZombies[i].enforcerZombie.GetComponent<EnforcerZombieAI>().GenerateNewPoint();
        }
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
        discreteAction[4] = Random.Range(0, 10);
        discreteAction[5] = Random.Range(0, 2);

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
