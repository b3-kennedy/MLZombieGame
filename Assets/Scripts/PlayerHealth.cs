using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public float timeToRegen;
    public float regenerationRate;
    float regenTimer;
    bool hasTakenDamage;
    bool canRegen;

    [Range(0.0f, 1.0f)]
    public float vignetteMultiplier;

    public AnimationCurve vignettePulse;

    [Range(0.0f, 0.1f)]
    public float vigPulseMultiplier;
    float baseVigValue;
    float vigTimer;
    float newVigVal;

    public Recoil camRecoil;
    public float flinchVal;

    public AudioSource hurtSource;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTakenDamage)
        {
            regenTimer += Time.deltaTime;
            if(regenTimer >= timeToRegen)
            {
                canRegen = true;
                hasTakenDamage = false;
            }
        }

        if (canRegen && currentHealth < maxHealth)
        {
            currentHealth += Time.deltaTime * regenerationRate;
        }
        else
        {
            canRegen = false;
        }

        if(PostProcessingManager.Instance != null)
        {
            baseVigValue = ((maxHealth - currentHealth) / 100) * vignetteMultiplier;
            if(baseVigValue > 0)
            {
                vigTimer += Time.deltaTime;
                newVigVal = baseVigValue + (vignettePulse.Evaluate(vigTimer) * vigPulseMultiplier);
            }
            if (vigTimer >= 1)
            {
                vigTimer = 0;
                newVigVal = baseVigValue;
            }
            PostProcessingManager.Instance.vignette.intensity.value = newVigVal;
        }
    }

    public void Flinch()
    {
        Debug.Log("flinch");
        hurtSource.Play();
        camRecoil.targetRot += new Vector3(flinchVal, 0, 0);
    }

    public void TakeDamage(float dmg)
    {
        if(currentHealth - dmg <= 0)
        {
            currentHealth = 0;
            if(GameManager.Instance != null)
            {
                GameManager.Instance.Lose();
            }
            
            if(MLPatrol2.Instance != null)
            {
                MLPatrol2.Instance.AddReward(2f);
                MLPatrol2.Instance.End();
            }
            Flinch();
            //gameObject.SetActive(false);
            
        }
        else
        {
            Flinch();
            currentHealth -= dmg;
            hasTakenDamage = true;
            canRegen = false;
        }
        
    }
}
