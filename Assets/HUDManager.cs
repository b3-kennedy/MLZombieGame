using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI gunNameText;


    private void Awake()
    {
        Instance = this;
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

}
