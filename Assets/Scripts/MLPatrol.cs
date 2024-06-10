using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

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

    Vector2 pos;


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
        player.transform.position = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 200));
        TakeAction();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int posX = actions.DiscreteActions[0] * 8;
        int posY = actions.DiscreteActions[1] * 8;

        float combinedDistance = 0;

        pos = new Vector2(posX, posY);

        Debug.Log(pos);
        RandomPatrol(pos);
        if(player != null)
        {
            for (int i = 0;i < groups.Count;i++)
            {
                if (Vector3.Distance(player.transform.position, groups[i].patrolPoint.position) < 50)
                {
                    AddReward(1);
                    Debug.Log("Found Player");
                    EndEpisode();
                }
                else
                {
                    AddReward(-0.1f);
                }

                combinedDistance += Vector3.Distance(player.transform.position, groups[i].patrolPoint.position);
            }
            
            if(combinedDistance < 50 * 5)
            {
                AddReward(3);
                EndEpisode();
            }
        }
        
        


    }

    void TargetedPatrol()
    {

    }

    void RandomPatrol(Vector2 pos)
    {
        int randomNum = Random.Range(0, groups.Count);
        groups[randomNum].patrolPoint.position = new Vector3(pos.x, 0, pos.y);
        foreach (var zombie in groups[randomNum].zombies)
        {
            zombie.GetComponent<ZombiePatrolAI>().patrolPoint = groups[randomNum].patrolPoint;
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
        timer += Time.deltaTime;
        if (timer >= patrolTime)
        {
            TakeAction();
            timer = 0;
        }
    }
}
