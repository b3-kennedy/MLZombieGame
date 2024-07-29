using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentSlot : MonoBehaviour
{
    public GameObject attachmentUI;
    public GameObject attachmentObj;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { OnClick(); });
    }

    public void OnClick()
    {
        if(attachmentObj != null)
        {
            InventoryManager.Instance.OnUnequipItem(attachmentUI, attachmentObj, GetComponent<Image>());
        }
        
    }
}
