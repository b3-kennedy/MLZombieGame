using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;
    public BossAI boss;
    public GameObject bossDoor;
    public GameObject bossDoor2;
    public GameObject bossHealthBar;
    public AudioClip doorOpen;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(HUDManager.Instance != null)
        {
            HUDManager.Instance.SetWhiteText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossDefeated()
    {
        bossHealthBar.SetActive(false);
        bossDoor2.GetComponent<Animator>().SetBool("open", true);
        AudioSource.PlayClipAtPoint(doorOpen, bossDoor2.transform.position);
    }

    public void StartFight()
    {
        boss.StartBossFight();
    }

    public void OpenBossDoor()
    {
        bossDoor.GetComponent<Animator>().SetBool("open", true);
    }

    public void CloseBossDoor()
    {
        bossDoor.GetComponent<Animator>().SetBool("close", true);
    }
}
