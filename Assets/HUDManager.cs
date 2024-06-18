using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public enum InteractionType {GRAB, BREAK};
    public InteractionType type;

    public static HUDManager Instance;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI gunNameText;
    public GameObject spottedText;
    public GameObject mlIdentifier;

    public GameObject interactPrompt;
    public Image interactIcon;

    public Sprite grabIcon;
    public Sprite breakIcon;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(mlIdentifier != null)
        {
            if (mlIdentifier.activeSelf)
            {
                spottedText.SetActive(true);
            }
            else
            {
                spottedText.SetActive(false);
            }
        }

    }

    private void Start()
    {
        ammoCountText.text = "";
        gunNameText.text = "";
    }

    public void UpdateAmmoText(int ammo, int mags)
    {
        ammoCountText.text = ammo.ToString() + "/" + mags.ToString();
    }

    public void UpdateGunText(string name)
    {
        gunNameText.text = name;
    }

    public void UpdateInteractPrompt(string text)
    {
        interactPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

    }

    public void ShowInteractPrompt()
    {
        interactPrompt.SetActive(true);  
    }

    public void HideInteractPrompt()
    {
        interactPrompt.SetActive(false);
    }

}
