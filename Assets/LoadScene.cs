using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int sceneIndex;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

    }

    public void LoadNewScene()
    {
        LevelManager.Instance.SwitchToScene(sceneIndex);
    }
}
