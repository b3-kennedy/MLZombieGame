using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableSlot : MonoBehaviour
{

    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnequipThrowable()
    {
        if (item != null)
        {
            GetComponent<Image>().color = Color.white;
            InventoryManager.Instance.PickUpThrowable(item);
            item = null;
        }
    }

    public void ItemThrown()
    {
        if(item != null)
        {
            GetComponent<Image>().color = Color.white;
            item = null;
        }
    }
}
