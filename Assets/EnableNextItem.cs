using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableNextItem : MonoBehaviour
{
    public GameObject[] itemsArray;
    int index = 0;

    private void Start()
    {
        itemsArray[0].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject item in itemsArray)
            {
                if(item != itemsArray[index])
                {
                    item.SetActive(false);
                }
                else
                {
                    item.SetActive(true);
                }
            }
            index++;
        }
    }
}
