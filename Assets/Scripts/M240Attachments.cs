using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M240Attachments : Attachments
{
    [Header("Barrel")]
    public GameObject barrelAttachment;
    public Transform barrelAttachmentPos;

    public AudioClip suppressedShotSound;


    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        if (barrelAttachment == null)
        {
            Destroy(barrelAttachment);
            GameObject newAttachment = Instantiate(attachment, barrelAttachmentPos);
            switch (gun)
            {
                case Gun.M240:
                    newAttachment.transform.localPosition = attachment.GetComponent<BarrelAttachment>().M240Position;
                    break;
            }

            newAttachment.transform.localRotation = Quaternion.Euler(0, 0, 0);
            barrelAttachment = newAttachment;
            Shoot currentGun = transform.GetChild(0).GetComponent<Shoot>();
            currentGun.audioRange /= 2;
            currentGun.shotSound = suppressedShotSound;
            currentGun.isSuppressed = true;
            InventoryManager.Instance.attachmentsList.Remove(button);
            Destroy(button);
        }

    }


    public override void UnequipBarrel()
    {
        Shoot currentGun = transform.GetChild(0).GetComponent<Shoot>();
        currentGun.shotSound = normalSound;
        currentGun.audioRange *= 2;
        currentGun.isSuppressed = false;
    }
}
