using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChamberEnterTrigger : MonoBehaviour
{
    public enum TriggerType {ENTER,EXIT,DOOR};
    public TriggerType type;

    private void OnTriggerExit(Collider other)
    {
        if (type == TriggerType.ENTER && other.GetComponent<RigidbodyMovement>())
        {
            ZombieSpawnerManager.Instance.exitTrigger.SetActive(true);
            ZombieSpawnerManager.Instance.enterTrigger.SetActive(false);
            if(ZombieSpawnerManager.Instance.zombie != null)
            {
                if (ZombieSpawnerManager.Instance.zombie.GetComponent<HunterZombieAI>())
                {
                    ZombieSpawnerManager.Instance.zombie.GetComponent<HunterZombieAI>().playerPos = HUDManager.Instance.transform;
                }
                
            }
            
        }
        else if(type == TriggerType.EXIT && other.GetComponent<RigidbodyMovement>())
        {
            ZombieSpawnerManager.Instance.exitTrigger.SetActive(false);
            ZombieSpawnerManager.Instance.enterTrigger.SetActive(true);
            ZombieSpawnerManager.Instance.doorAnim.SetBool("open", false);
        }

        if (other.transform.root.GetComponent<HunterZombieAI>() || other.transform.root.GetComponent<EnforcerZombieAI>() || other.transform.root.GetComponent<ZombiePatrolAI>())
        {
            Destroy(other.gameObject);
        }
    }
}
