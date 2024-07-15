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
                GameObject point = Instantiate(critPoint, tempList[randomNum].position, Quaternion.identity);
                point.transform.SetParent(critPositions[randomNum]);
                spawnedCritPos.Add(point);
                tempList.RemoveAt(randomNum);
            }
            yield return new WaitForSeconds(0.5f);

        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
