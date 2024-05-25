using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlockAttachments : Attachments
{

    [Header("Barrel")]
    public GameObject barrelAttachment;
    public Transform barrelAttachmentPos;

    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        Destroy(barrelAttachment);
        GameObject newAttachment = Instantiate(attachment, barrelAttachmentPos);
        switch (gun)
        {
            case Gun.GLOCK:
                newAttachment.transform.localPosition = attachment.GetComponent<BarrelAttachment>().M4Position;
                break;
        }

        newAttachment.transform.localRotation = Quaternion.Euler(0, 0, 0);
        barrelAttachment = newAttachment;
        Destroy(button);
    }
}
