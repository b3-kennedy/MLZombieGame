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

    public int numberOfBandages;
    public float bandageTime;
    public float healAmount;
    public GameObject healBar;
    public GameObject healUI;
    float bandageTimer;
    bool isBandaging;
    Animator anim;
    Shoot shootScript;

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
        anim = GetComponent<PickUpWeapons>().weaponPos.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B) && !isBandaging && numberOfBandages > 0)
        {
            anim.SetBool("bandaging", true);
            if(GetComponent<PickUpWeapons>().currentActiveGun != null)
            {
                GetComponent<PickUpWeapons>().currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().canShoot = false;
            }
            
            healUI.SetActive(true);
            isBandaging = true;
            GetComponent<RigidbodyMovement>().canSprint = false;
            
        }

        if (isBandaging)
        {

            GetComponent<RigidbodyMovement>().playerState = RigidbodyMovement.PlayerState.BANDAGING;
            bandageTimer += Time.deltaTime;
            healBar.GetComponent<RectTransform>().localScale = new Vector3(bandageTimer / bandageTime, healBar.GetComponent<RectTransform>().localScale.y,
                healBar.GetComponent<RectTransform>().localScale.z);
            if (currentHealth < 100)
            {
                currentHealth += healAmount;
            }
            
            if(bandageTimer >= bandageTime)
            {
                anim.SetBool("bandaging", false);
                healUI.SetActive(false);
                healBar.GetComponent<RectTransform>().localScale = new Vector3(0, healBar.GetComponent<RectTransform>().localScale.y,
                    healBar.GetComponent<RectTransform>().localScale.z);
                currentHealth += healAmount;
                numberOfBandages -= 1;
                GetComponent<InventoryManager>().UpdateBandagesText();
                bandageTimer = 0;
                if (GetComponent<PickUpWeapons>().currentActiveGun != null)
                {
                    GetComponent<PickUpWeapons>().currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().canShoot = true;
                }
                GetComponent<RigidbodyMovement>().canSprint = true;
                GetComponent<RigidbodyMovement>().playerState = RigidbodyMovement.PlayerState.NORMAL;
                isBandaging = false;
            }
        }

        if (hasTakenDamage && !canRegen)
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
        if (camRecoil != null)
        {
            Debug.Log("flinch");
            hurtSource.Play();
            camRecoil.targetRot += new Vector3(flinchVal, 0, 0);
        }

    }

    public void TakeDamage(float dmg)
    {
        if(currentHealth - dmg <= 0)
        {
            currentHealth = 0;
            if(GameManager.Instance != null && !GetComponent<PlayerAIMove>())
            {
                GameManager.Instance.Lose();
            }
            
            if(MLPatrol2.Instance != null)
            {
                MLPatrol2.Instance.AddReward(2f);
                MLPatrol2.Instance.End();
            }

            if (GetComponent<PlayerAIMove>())
            {
                GetComponent<PlayerAIMove>().mlIdentifier.SetActive(true);
                GetComponent<PlayerAIMove>().mlIdentifierTimer = 0;
            }
            Flinch();
            //gameObject.SetActive(false);
            
        }
        else
        {
            Flinch();
            currentHealth -= dmg;
            hasTakenDamage = true;
            regenTimer = 0;
            canRegen = false;
        }
        
    }
}
