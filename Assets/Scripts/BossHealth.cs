using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public GameObject critPoint;
    public List<Transform> critPositions;
    public List<GameObject> spawnedCritPos;
    public GameObject healthBar;

    public Collider[] colliders;

    public bool isDead = false;

    public float critSpawnTime;
    float critSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        critSpawnTimer += Time.deltaTime;
        if(critSpawnTimer >= critSpawnTime)
        {
            StartCoroutine(SpawnCritPoints());
            critSpawnTimer = 0;
        }
    }

    public void ClearNullPositions()
    {
        for(int i = 0; i < spawnedCritPos.Count; i++)
        {
            if (spawnedCritPos[i] == null)
            {
                spawnedCritPos.RemoveAt(i); ;
            }
        }
    }



    IEnumerator SpawnCritPoints()
    {
        var tempList = new List<Transform>(critPositions);
        for (int i = 0; i < 3; i++)
        {
            if (spawnedCritPos.Count < 4)
            {
                int randomNum = Random.Range(0, tempList.Count);
                if (tempList[randomNum].childCount < 1)
                {
                    GameObject point = Instantiate(critPoint, tempList[randomNum].position, Quaternion.identity);
                    point.transform.SetParent(tempList[randomNum]);
                    spawnedCritPos.Add(point);
                    tempList.RemoveAt(randomNum);
                }

            }
            yield return new WaitForSeconds(0.5f);

        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0 && !isDead)
        {
            BossFightManager.Instance.BossDefeated();
            GetComponent<Animator>().SetTrigger("dead");
            GetComponent<Animator>().SetBool("throw", false);
            GetComponent<Animator>().SetBool("charge", false);
            GetComponent<Animator>().SetBool("wave", false);
            GetComponent<Animator>().SetBool("stomp", false);
            GetComponent<BossAI>().enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            isDead = true;

            foreach (var col in colliders)
            {
                col.enabled = false;
            }
        }
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(currentHealth / maxHealth, healthBar.GetComponent<RectTransform>().localScale.y,
            healthBar.GetComponent<RectTransform>().localScale.z);
        
    }
}
