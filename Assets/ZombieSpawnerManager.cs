using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerManager : MonoBehaviour
{

    public static ZombieSpawnerManager Instance;

    public Transform zombieSpawn;

    public GameObject scoutZombie;
    public GameObject hunterZombie;
    public GameObject enforcerZombie;
    public Animator doorAnim;
    [HideInInspector] public GameObject zombie;
    public GameObject exitTrigger;
    public GameObject enterTrigger;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        exitTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnScout()
    {
        if(zombie != null)
        {
            Destroy(zombie);
        }
        zombie = Instantiate(scoutZombie, zombieSpawn.position, Quaternion.identity);
        zombie.GetComponent<ZombiePatrolAI>().patrolPoint = zombieSpawn;
        doorAnim.SetBool("open", true);
    }

    public void SpawnEnforcer()
    {
        if (zombie != null)
        {
            Destroy(zombie);
        }
        zombie = Instantiate(enforcerZombie, zombieSpawn.position, Quaternion.identity);
        zombie.GetComponent<EnforcerZombieAI>().patrolPoint = zombieSpawn;
        doorAnim.SetBool("open", true);
    }

    public void SpawnHunter()
    {
        if (zombie != null)
        {
            Destroy(zombie);
        }
        zombie = Instantiate(hunterZombie, zombieSpawn.position, Quaternion.identity);
        doorAnim.SetBool("open", true);
    }
}
