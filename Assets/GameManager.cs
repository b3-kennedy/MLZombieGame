using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject bossLoseScreen;
    public GameObject player;
    public bool gameOver;
    public bool hasRedKey;
    public bool hasGreenKey;
    public bool hasBlueKey;

    public bool activate;
    public GameObject[] scoutZombies;
    public List<GameObject> hunterZombies;
    public float scoutTimer;

    public GameObject smokeCamera;

    public bool isTraining;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (isTraining)
        {
            Debug.LogWarning("Training mode is on");
        }
    }

    public void PlayerOpeningDoor()
    {
        foreach (GameObject zombie in hunterZombies)
        {
            zombie.GetComponent<HunterZombieAI>().DoorUnlocking();
        }
    }

    public void EnableSmokeCamera()
    {
        smokeCamera.SetActive(true);
    }

    public void DisableSmokeCamera()
    {
        smokeCamera.SetActive(false);
    }

    public void Win()
    {
        gameOver = true;
        Cursor.lockState = CursorLockMode.Confined;
        winScreen.SetActive(true);
    }

    private void Update()
    {
        if (gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //foreach (var zombie in scoutZombies)
        //{
        //    if (Vector3.Distance(zombie.transform.position, player.transform.position) > 21f)
        //    {
        //        zombie.GetComponent<ScoutZombieAudioManager>().footstepSource.enabled = false;
        //    }
        //    else
        //    {
        //        zombie.GetComponent<ScoutZombieAudioManager>().footstepSource.enabled = true;
        //    }
        //}

        //if (!activate)
        //{
        //    scoutTimer += Time.deltaTime;
        //    if (scoutTimer >= 1)
        //    {
        //        foreach (var zombie in scoutZombies)
        //        {
        //            zombie.SetActive(true);
        //        }
        //        activate = true;
        //    }

        //}


    }

    public void SwitchToBossScene()
    {

        player.GetComponent<Rigidbody>().position = new Vector3(0, 1, -21);
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SwitchToScene(3);
        }
        PauseMenuManager.Instance.player = player.transform.parent.gameObject;
        DontDestroyOnLoad(player.transform.parent);
        
    }

    public void MovePlayerPos()
    {
        player.transform.parent.position = new Vector3(0, 1, -21);
        player.transform.localPosition = Vector3.zero;
        Debug.Log("switched");
    }

    public void Lose()
    {
        gameOver = true;
        Cursor.lockState = CursorLockMode.Confined;
        if(BossFightManager.Instance != null)
        {
            if (!BossFightManager.Instance.checkpoint)
            {
                loseScreen.SetActive(true);
            }
            else
            {
                bossLoseScreen.SetActive(true);
            }
        }
        else
        {
            loseScreen.SetActive(true);
        }

        
    }

    public void Restart()
    {
        
        LevelManager.Instance.SwitchToScene(1);
        OnRestart();
        
    }

    public void BossRestart()
    {
        
        LevelManager.Instance.SwitchToScene(3);
        bossLoseScreen.SetActive(false);
        player.GetComponent<PlayerHealth>().currentHealth = 99f;
        PostProcessingManager.Instance.vignette.intensity.value = 0f;
        player.GetComponent<Rigidbody>().MovePosition(new Vector3(0, 1, -21));
        Cursor.lockState = CursorLockMode.Locked;
        gameOver = false;
    }

    public void OnRestart()
    {
        Destroy(gameObject);
        Destroy(player.transform.parent.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        LevelManager.Instance.SwitchToScene(0);
        OnRestart();
        Debug.Log("go to main menu");
    }
}
