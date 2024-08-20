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
    public float maxThreat;

    public int hunterCount;

    public GameObject scout;
    public GameObject hunter;
    public GameObject enforcer;

    public Transform hunterSpawn;

    public Transform hunterParent;


    public enum ThreatState {MIN, MAX};
    public ThreatState threatState;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //public override void OnEpisodeBegin()
    //{
    //    if (player != null)
    //    {
            
    //        //player.transform.position = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 200));
    //        //threatValue = 100 - (Vector3.Distance(brainPos.position, player.transform.position) / 2);
    //    }

    //    //TakeAction();
    //}

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    sensor.AddObservation(threatValue);
    //}

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (player != null)
    //    {
    //        threatValue = 100 - (Vector3.Distance(brainPos.position, player.transform.position)/ 315.5935f) * 100;
    //    }
        

    //    if(threatValue < minThreat)
    //    {
    //        threatState = ThreatState.MIN;
    //    }
    //    else if(threatValue < minThreat)
    //    {
    //        threatState = ThreatState.MAX;
    //    }

    //    RequestDecision();

    //}

    public void SpawnHunter()
    {
        for (int i = 0; i < hunterCount; i++)
        {
            if (GameManager.Instance.player.activeSelf)
            {
                GameObject hunterZombie = Instantiate(hunter, hunterSpawn.position, Quaternion.identity);
                GameManager.Instance.hunterZombies.Add(hunterZombie);
                hunterZombie.GetComponent<HunterZombieAI>().playerPos = player;
                hunterZombie.GetComponent<HunterZombieAI>().home = hunterSpawn;

            }

        }
    }

    //public override void OnActionReceived(ActionBuffers actions)
    //{
    //    int action = actions.DiscreteActions[0];

    //    Spawn(action);
    //}

    //void Spawn(int action)
    //{

    //    //Debug.Log(action);
    //    if (action == 0)
    //    {
    //        if(threatState == ThreatState.MIN)
    //        {
    //            AddReward(1);
    //        }
    //        else
    //        {
    //            AddReward(-0.01f);
    //        }
    //        //Debug.Log("spawn patrol");
    //    }
    //    else if(action == 1)
    //    {
            
    //        if (threatState == ThreatState.MAX)
    //        {
    //            AddReward(1);
    //        }
    //        else
    //        {
    //            AddReward(-0.01f);
    //        }
    //        //Debug.Log("spawn enforcer");
    //    }
    //    End();
    //}

    //public void End()
    //{
    //    EndEpisode();
    //}


}
