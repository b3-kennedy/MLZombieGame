using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHUDToWhite : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if(HUDManager.Instance != null)
        {
            HUDManager.Instance.SetWhiteText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
