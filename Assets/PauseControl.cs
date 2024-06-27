using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuManager.Instance != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenuManager.Instance.OnPause();
            }

            if (PauseMenuManager.Instance.isPaused)
            {
                GetComponent<MouseLook>().enabled = false;
            }
            else
            {
                GetComponent<MouseLook>().enabled = true;
            }
        }

    }
}
