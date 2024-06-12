using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLSpawn : Agent
{

    public static MLSpawn Instance;

    public Transform player;
    public Transform brainPos;
    public float threatValue;
    public float minThreat;
    public float mediumThreat;
    public float maxThreat;
    public enum ThreatState {MIN, MEDIUM, MAX};
    public ThreatState threatState;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        if (player != null)
        {
            
            player.transform.position = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 200));
            threatValue = 100 - (Vector3.Distance(brainPos.position, player.transform.position) / 2);
        }

        //TakeAction();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(threatValue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            threatValue = 100 - (Vector3.Distance(brainPos.position, player.transform.position) / 2);
        }
        

        if(threatValue < minThreat)
        {
            threatState = ThreatState.MIN;
        }
        else if(threatValue > minThreat && threatValue < mediumThreat)
        {
            threatState = ThreatState.MEDIUM;
        }
        else if(threatValue < maxThreat && threatValue > mediumThreat)
        {
            threatState = ThreatState.MAX;
        }

        RequestDecision();

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        Spawn(action);
    }

    void Spawn(int action)
    {
        if(action == 0)
        {
            if(threatState == ThreatState.MIN)
            {
                AddReward(1);
            }
            else
            {
                AddReward(-0.01f);
            }
            Debug.Log("spawn patrol");
        }
        else if(action == 1)
        {
            if (threatState == ThreatState.MEDIUM)
            {
                AddReward(1);
            }
            else
            {
                AddReward(-0.01f);
            }
            Debug.Log("spawn hunter");
        }
        else if(action == 2)
        {
            if (threatState == ThreatState.MAX)
            {
                AddReward(1);
            }
            else
            {
                AddReward(-0.01f);
            }
            Debug.Log("spawn enforcer");
        }
        End();
    }

    public void End()
    {
        EndEpisode();
    }


}
