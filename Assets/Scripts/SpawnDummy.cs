using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummy : MonoBehaviour
{
    public static SpawnDummy Instance;

    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    public GameObject dummy;
    GameObject newDummy;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        //if(newDummy == null)
        //{
        //    newDummy = Instantiate(dummy, new Vector3(Random.Range(topLeft.position.x, bottomLeft.position.x), 0, Random.Range(topLeft.position.z, topRight.position.z)), Quaternion.Euler(0, -90, 0));
        //}
        //else
        //{
        //    newDummy.transform.position = new Vector3(Random.Range(topLeft.position.x, bottomLeft.position.x), 0, Random.Range(topLeft.position.z, topRight.position.z));
        //    newDummy.GetComponent<Health>().health = 100;
            
        //}

        //if (!newDummy.activeSelf)
        //{
        //    newDummy.SetActive(true);
        //}
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
