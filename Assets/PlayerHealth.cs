using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        if(currentHealth - dmg <= 0)
        {
            GameManager.Instance.Lose();
            gameObject.SetActive(false);
            currentHealth = 0;
        }
        else
        {
            currentHealth -= dmg;
        }
    }
}
