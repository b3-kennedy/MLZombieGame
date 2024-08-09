using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBossElements : MonoBehaviour
{

    public GameObject bossHealthBar;
    public BossAI bossAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            bossAI.target = other.transform;
            BossFightManager.Instance.StartFight();
            BossFightManager.Instance.CloseBossDoor();
            bossHealthBar.SetActive(true);
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(BossFightManager.Instance.doorOpen, transform.position);
        }
    }
}
