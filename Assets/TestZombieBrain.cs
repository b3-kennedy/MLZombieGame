using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class TestZombieBrain : Agent
{
    public Transform target;
    public List<GameObject> zombies;
    int spawnedZombies;


    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(spawnedZombies);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float moveSpeed = 2;

        transform.localPosition += new Vector3(move, 0f, 0f) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.;
        continuousActions[0] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target"))
        {
            AddReward(50);
            EndEpisode();
        }

        if (other.CompareTag("Wall"))
        {
            AddReward(-5);
            EndEpisode();
        }
    }

    public void SpawnUnit()
    {
        Debug.Log("spawned");
        AddReward(1);

    }
}
