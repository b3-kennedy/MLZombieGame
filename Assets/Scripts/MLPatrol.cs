using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Grpc.Core;

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

    public float patrolTime;
    float timer;


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
        Debug.Log("action");
        RequestAction();
    }

    public override void OnEpisodeBegin()
    {
        //player.transform.position = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 200));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int posX = actions.DiscreteActions[0] * 8;
        int posY = actions.DiscreteActions[1] * 8;

        Vector2 pos = new Vector2(posX, posY);

        Debug.Log(pos);

        if(Physics.Raycast(new Vector3(pos.x, 10, pos.y), -Vector3.up, out RaycastHit hit, 100, sphereLayer))
        {
            if (hit.collider)
            {
                AddReward(-0.01f);
                Debug.Log("hit: " + hit.collider.name);
            }

        }
        else
        {
            Debug.Log("not hit");
            int randomNum = Random.Range(0, groups.Count);
            groups[randomNum].patrolPoint.position = new Vector3(pos.x, 0, pos.y);
            foreach (var zombie in groups[randomNum].zombies)
            {
                zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[randomNum].patrolPoint;
            }
            if (Vector3.Distance(groups[randomNum].patrolPoint.position, player.transform.position) < 25)
            {
                AddReward(2f);
                EndEpisode();
            }
            AddReward(1f);
            EndEpisode();
        }


    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = Random.Range(0, 24);
        discreteAction[1] = Random.Range(0, 24);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeAction();
        }
        //timer += Time.deltaTime;
        //if(timer >= patrolTime)
        //{
        //    TakeAction();
        //    timer = 0;
        //}
    }
}
