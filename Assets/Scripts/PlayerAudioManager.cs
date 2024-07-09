using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{

    public AudioSource footstepSource;

    public AudioClip[] concreteStepsWalk;
    public AudioClip[] grassStepsWalk;

    // Start is called before the first frame update
    void Start()
    {

    }



    public void PlayFootstep(AudioClip[] type)
    {
        int randomNum = Random.Range(0, type.Length);
        footstepSource.clip = type[randomNum];
        footstepSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
