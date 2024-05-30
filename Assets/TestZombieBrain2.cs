using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZombieBrain2 : MonoBehaviour
{

    public static TestZombieBrain2 Instance;

    public List<GameObject> hunterZombies;



    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hunt(Transform pos)
    {
        foreach (GameObject zombie in hunterZombies)
        {
            zombie.GetComponent<HunterZombieAI>().playerPos = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
