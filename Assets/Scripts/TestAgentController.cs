using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TestAgentController : Agent
{

    public Transform target;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.44f, 0);
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            target.localPosition = new Vector3(-4f, 0.3f, 0f);
        }
        else
        {
            target.localPosition = new Vector3(4f, 0.3f, 0f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float moveSpeed = 2;

        transform.localPosition += new Vector3(move, 0f, 0f) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target"))
        {
            AddReward(10);
            EndEpisode();
        }

        if (other.CompareTag("Wall"))
        {
            AddReward(-5);
            EndEpisode();
        }
    }
}
