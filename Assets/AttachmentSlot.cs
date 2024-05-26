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
        GetComponent<Button>().onClick.AddListener(delegate { InventoryManager.Instance.OnUnequipItem(attachmentUI, attachmentObj, GetComponent<Image>()); });
    }
}
