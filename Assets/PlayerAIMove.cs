using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAIMove : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;
    public int index;
    public List<Transform> positions;
    List<Transform> activePositions;

    public Transform key1;
    public Transform key2;
    public Transform key3;


    private void Awake()
    {
        activePositions = positions;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(target.position);
        
    }

    public void RandomSpawn()
    {

    }

    public void OnReset()
    {
        positions.Clear();
        positions.Add(key1);
        positions.Add(key2);
        positions.Add(key3);
    }

    public void PickPos()
    {
        index = Random.Range(0, positions.Count);
        if (positions.Count > 0)
        {
            if(agent != null)
            {
                agent.SetDestination(positions[index].position);
            }
            
        }
        else
        {
            if(agent != null)
            {
                agent.SetDestination(target.position);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activePositions.Count > 0)
        {
            if (Vector3.Distance(transform.position, positions[index].position) < 2f)
            {
                positions.RemoveAt(index);
                PickPos();
                MLPatrol2.Instance.AddReward(-5f);

            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.position) < 5f)
            {

                agent.ResetPath();
                Debug.Log("end");
                MLPatrol2.Instance.AddReward(-15f);
                MLPatrol2.Instance.End();
            }
        }



    }
}
