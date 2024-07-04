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

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        //gameOver = true;
        //Cursor.lockState = CursorLockMode.Confined;
        //winScreen.SetActive(true);
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

    public void Lose()
    {
        //gameOver = true;
        //Cursor.lockState = CursorLockMode.Confined;
        //loseScreen.SetActive(true);
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
