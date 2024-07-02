using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{

    public static PostProcessingManager Instance;

    public UnityEngine.Rendering.Volume volume;

    [HideInInspector] public Vignette vignette;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
