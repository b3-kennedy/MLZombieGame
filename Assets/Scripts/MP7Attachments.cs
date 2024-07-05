using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP7Attachments : Attachments
{


    [Header("Barrel")]
    public GameObject barrelAttachment;
    public Transform barrelAttachmentPos;

    [Header("Sight")]
    public GameObject sightAttachment;
    public Transform sightAttachmentPos;

    Shoot shootScript;

    AudioClip normalShot;
    public AudioClip suppressedShotSound;

    private void Start()
    {
        shootScript = transform.GetChild(0).GetComponent<Shoot>();
    }

    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        if (barrelAttachment == null)
        {
            Destroy(barrelAttachment);
            GameObject newAttachment = Instantiate(attachment, barrelAttachmentPos);
            switch (gun)
            {
                case Gun.MP7:
                    newAttachment.transform.localPosition = attachment.GetComponent<BarrelAttachment>().mp7Position;
                    break;
            }

            newAttachment.transform.localRotation = Quaternion.Euler(0, 0, 0);
            barrelAttachment = newAttachment;
            Shoot currentGun = transform.GetChild(0).GetComponent<Shoot>();
            currentGun.shotSound = suppressedShotSound;
            currentGun.audioRange /= 2;
            InventoryManager.Instance.attachmentsList.Remove(button);
            Destroy(button);
        }

    }

    public override void AttachSight(GameObject attachment, GameObject button)
    {
        if (sightAttachment == null)
        {

            Destroy(sightAttachment);
            GameObject newAttachment = Instantiate(attachment, sightAttachmentPos);
            newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().M4position;
            newAttachment.transform.rotation = sightAttachmentPos.rotation;
            sightAttachment = newAttachment;

            //if (ironSights.Length > 0)
            //{
            //    ironSights[0].SetActive(false);
            //    ironSights[1].transform.localRotation = Quaternion.Euler(90, 0, 0);
            //}
            InventoryManager.Instance.attachmentsList.Remove(button);
            Destroy(button);
        }

    }

    public override void UnequipBarrel()
    {
        Shoot currentGun = transform.GetChild(0).GetComponent<Shoot>();
        currentGun.shotSound = normalShot;
        currentGun.audioRange *= 2;
    }
}
