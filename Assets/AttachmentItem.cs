using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : MonoBehaviour
{
    public GameObject item;
    public AttachmentSlotType.AttachmentType attachmentType;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
