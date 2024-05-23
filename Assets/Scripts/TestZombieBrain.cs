using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;

public class TestZombieBrain : Agent
{
    public Transform target;
    public List<GameObject> zombies;
    public float levelSizeX;
    public float levelSizeZ;
    int spawnedZombies;
    float prevDist = 999;
    Vector3 pos;
    public NavMeshAgent agent;
    float timer;


    public override void OnEpisodeBegin()
    {
        target.transform.localPosition = new Vector3(Random.Range(-9, 9), 0.573f, Random.Range(-9, 9));
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(pos);
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(agent.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float x = actions.ContinuousActions[0] * levelSizeX;
        float z = actions.ContinuousActions[1] * levelSizeZ;

        timer += Time.deltaTime;
        if (timer > 3)
        {
            pos = new Vector3(x, 0, z);
            float dist = Vector3.Distance(pos, target.transform.position);

            agent.SetDestination(pos);

            timer = 0;
        }

        

        //Vector3 targetPos = new Vector3(target.localPosition.x, 0, target.localPosition.z);
        //float dist = Vector3.Distance(pos, targetPos);
    }

    public void End()
    {
        EndEpisode();
    }

    public void GiveReward(float reward)
    {
        AddReward(reward);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("test");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 300f))
            {
                if (hit.transform)
                {
                    Debug.Log(hit);
                }
            }
        }


        //continuousActions[0] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void SpawnUnit()
    {
        Debug.Log("spawned");
        AddReward(1);

    }
}
