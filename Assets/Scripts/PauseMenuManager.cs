using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;
    public GameObject pauseCanvas;
    [HideInInspector] public bool isPaused;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
        
    }

    public void OnPause()
    {
        Debug.Log("pause");
        if (!pauseCanvas.activeSelf)
        {
            Time.timeScale = 0.0001f;
            pauseCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
    }

    public void Resume()
    {
        OnPause();
    }

    public void MainMenu()
    {
        OnPause();
        Destroy(gameObject);
        LevelManager.Instance.SwitchToScene(0);
    }

    public void Settings()
    {
        if(SettingsMenuManager.Instance != null)
        {
            SettingsMenuManager.Instance.OpenSettings();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
