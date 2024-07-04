using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public static SettingsMenuManager Instance;

    public GameObject settingsCanvas;

    [Header("Sensitivity Slider")]
    public float mouseSensValue;
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI sliderValueText;
    public TMP_InputField sensInputField;


    [Header("Gun Volume Slider")]
    public float gunVolumeValue;
    public Slider gunVolumeSlider;
    public TextMeshProUGUI gunVolumeSliderText;
    public TMP_InputField gunVolumeInputField;

    [HideInInspector] public UnityEvent updatedSens;
    [HideInInspector] public UnityEvent updatedGunVolume;

    string path = "Assets/Resources/Settings.txt";

    void Awake()
    {
        

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!File.Exists(path))
        {
            File.CreateText(path);
        }
        else
        {
            var file = Resources.Load<TextAsset>("Settings");

            string text = file.text;
            string[] lines = Regex.Split(text, "\n");

            foreach (string line in lines) 
            {
                Debug.Log(line);
            }
            
            mouseSensValue = float.Parse(lines[0]);
            gunVolumeValue = float.Parse(lines[1]);
            mouseSensitivitySlider.value = mouseSensValue / 100f;
            gunVolumeSlider.value = gunVolumeValue / 100f;

        }
    }

    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }

    public void UpdateMouseSensitivity()
    {
        float val = mouseSensitivitySlider.value * 100;
        mouseSensValue = Mathf.Round(val * 10f)/10f;

        sliderValueText.text = mouseSensValue.ToString();
        updatedSens.Invoke();
    }

    public void UpdateMouseSensitivityFromInput()
    {
        mouseSensValue = float.Parse(sensInputField.text);
        mouseSensitivitySlider.value = mouseSensValue / 100f;
        updatedSens.Invoke();
    }

    public void UpdateGunShotVolume()
    {
        float val = gunVolumeSlider.value * 100;
        gunVolumeValue = Mathf.Round(val * 10f) / 10f;

        gunVolumeSliderText.text = gunVolumeValue.ToString();
        updatedGunVolume.Invoke();
    }

    public void UpdateGunShotVolumeFromInput()
    {
        gunVolumeValue = float.Parse(gunVolumeInputField.text);
        gunVolumeSlider.value = gunVolumeValue / 100f;
        updatedGunVolume.Invoke();
    }

    public void WriteSettingsToFile()
    {
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(mouseSensValue.ToString());
        writer.WriteLine(gunVolumeValue.ToString());
        writer.Close();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
