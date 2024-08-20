using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScoutZombieAudioManager : PlayerAudioManager
{

    

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.muteAudio.AddListener(Mute);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Mute()
    {
        footstepSource.mute = true;
    }
}
