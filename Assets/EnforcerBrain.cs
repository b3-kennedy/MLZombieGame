using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;



public class EnforcerBrain : Agent
{
    public static EnforcerBrain Instance;
    public Transform[] playerSpawns;
    public List<EnforcerPatrol> enforcerZombies;
    public LayerMask sphereLayer;
    public Transform player;
    PlayerHealth playerHealth;
    public float randomPatrolTime;
    float randomPatrolTimer;

    public Transform key1Pos;
    public Transform key2Pos;
    public Transform key3Pos;
    public Transform barnPos;

    public Vector2 key1GridPos;
    public Vector2 key2GridPos;
    public Vector2 key3GridPos;
    public Vector2 barnGridPos;

    public float patrolTime;
    float timer;

    Vector2 pos;

    public List<Vector3> enforcerZombieStartingPatrolPos;

    public GameObject testPoint;


    private void Awake()
    {
        Instance = this;
        key1GridPos = new Vector2(Mathf.RoundToInt(key1Pos.position.x/9.5f), Mathf.RoundToInt(key1Pos.position.z/9.5f));
        key2GridPos = new Vector2(Mathf.RoundToInt(key2Pos.position.x / 9.5f), Mathf.RoundToInt(key2Pos.position.z / 9.5f));
        key3GridPos = new Vector2(Mathf.RoundToInt(key3Pos.position.x / 9.5f), Mathf.RoundToInt(key3Pos.position.z / 9.5f));
        barnGridPos = new Vector2(Mathf.RoundToInt(barnPos.position.x / 9.5f), Mathf.RoundToInt(barnPos.position.z / 9.5f));
    }

    void ResetZombiePositions()
    {
        key1GridPos = new Vector2(Mathf.RoundToInt(key1Pos.position.x / 9.5f), Mathf.RoundToInt(key1Pos.position.z / 9.5f));
        key2GridPos = new Vector2(Mathf.RoundToInt(key2Pos.position.x / 9.5f), Mathf.RoundToInt(key2Pos.position.z / 9.5f));
        key3GridPos = new Vector2(Mathf.RoundToInt(key3Pos.position.x / 9.5f), Mathf.RoundToInt(key3Pos.position.z / 9.5f));
        barnGridPos = new Vector2(Mathf.RoundToInt(barnPos.position.x / 9.5f), Mathf.RoundToInt(barnPos.position.z / 9.5f));
        for (int i = 0; i < enforcerZombies.Count; i++)
        {
            enforcerZombies[i].enforcerZombie.SetActive(true);
            enforcerZombies[i].patrolPoint.position = enforcerZombieStartingPatrolPos[i];
            enforcerZombies[i].enforcerZombie.transform.position = enforcerZombieStartingPatrolPos[i];
            enforcerZombies[i].enforcerZombie.GetComponent<NavMeshAgent>().ResetPath();
        }

        key1Pos.gameObject.SetActive(true);
        key2Pos.gameObject.SetActive(true);
        key3Pos.gameObject.SetActive(true);
        


    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.parent.GetComponent<PlayerHealth>();

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
        int posX = actions.DiscreteActions[0];
        int posY = actions.DiscreteActions[1];
        int enforcerNum = actions.DiscreteActions[2];
        int keyVal = actions.DiscreteActions[3];
        int enforcerBoolVal = actions.DiscreteActions[4];


        pos = new Vector2(posX, posY);

        List<Vector3> points = new List<Vector3>();


        if(enforcerBoolVal == 0)
        {
            if (Vector3.Distance(enforcerZombies[enforcerNum].patrolPoint.position, key1GridPos) <= 5 || Vector3.Distance(enforcerZombies[enforcerNum].patrolPoint.position, key2GridPos) <= 5 
                || Vector3.Distance(enforcerZombies[enforcerNum].patrolPoint.position, key3GridPos) <= 5)
            {
                AddReward(6f);
            }
        }
        else
        {
            RandomEnforcerPatrol(pos, enforcerNum);
        }

        //Debug.Log(Vector2.Distance(new Vector2(testPoint.transform.position.x / 9.5f, testPoint.transform.position.z / 9.5f), key1GridPos));
    }


    void TargetedPatrol()
    {

    }

    void RandomEnforcerPatrol(Vector2 pos, int group)
    {

        if(!key1Pos.gameObject.activeSelf)
        {
            key1GridPos = new Vector2(999, 999);
        }

        if(!key2Pos.gameObject.activeSelf)
        {
            key2GridPos = new Vector2(999, 999);
        }

        if(!key3Pos.gameObject.activeSelf)
        {
            key3GridPos = new Vector2(999, 999);
        }


        enforcerZombies[group].patrolPoint.position = new Vector3(Mathf.RoundToInt(pos.x*9.5f), 0, Mathf.RoundToInt(pos.y*9.5f));
        
        if(Vector2.Distance(pos, key1GridPos) <= 5)
        {
            AddReward(2f);
        }
        else
        {
            AddReward(-0.5f);
        }

        if (Vector2.Distance(pos, key2GridPos) <= 5)
        {
            AddReward(2f);
        }
        else
        {
            AddReward(-0.5f);
        }

        if (Vector2.Distance(pos, key3GridPos) <= 5)
        {
            AddReward(2f);
        }
        else
        {
            AddReward(-0.5f);
        }

        if(Vector2.Distance(pos, barnGridPos) <= 5)
        {
            AddReward(2f);
        }
        else
        {
            AddReward(-0.5f);
        }




        for (int i = 0; i < enforcerZombies.Count; i++)
        {
            enforcerZombies[i].enforcerZombie.GetComponent<EnforcerZombieAI>().patrolPoint = enforcerZombies[group].patrolPoint;
            enforcerZombies[i].enforcerZombie.GetComponent<EnforcerZombieAI>().GenerateNewPoint();
        }
    }

    //void RandomPatrol(Vector2 pos, int group)
    //{

    //    groups[group].patrolPoint.position = new Vector3(pos.x, 0, pos.y);


    //    foreach (var zombie in groups[group].zombies)
    //    {
    //        zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[group].patrolPoint;
    //        zombie.GetComponent<ZombiePatrolAI>().GenerateNewPoint();
    //    }



    //    //AddReward(1f);
    //}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = Random.Range(0, 24);
        discreteAction[1] = Random.Range(0, 24);
        discreteAction[2] = Random.Range(0, 10);
        discreteAction[3] = Random.Range(0, 3);
        discreteAction[4] = Random.Range(0, 2);



        //int posX = actions.DiscreteActions[0];
        //int posY = actions.DiscreteActions[1];
        //int enforcerNum = actions.DiscreteActions[2];
        //int keyVal = actions.DiscreteActions[3];
        //int enforcerBoolVal = actions.DiscreteActions[4];
    }

    public void GainReward(float points)
    {
        AddReward(points);
    }

    public void End()
    {
        Debug.Log("end");
        ResetPosition();

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
