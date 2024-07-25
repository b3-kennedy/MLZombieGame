using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject player;
    public bool gameOver;
    public bool hasRedKey;
    public bool hasGreenKey;
    public bool hasBlueKey;

    public bool activate;
    public GameObject[] scoutZombies;
    public float scoutTimer;

    public GameObject smokeCamera;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(winScreen.transform.parent.gameObject);
    }

    private void Start()
    {
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
        loseScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Debug.Log("go to main menu");
    }
}
