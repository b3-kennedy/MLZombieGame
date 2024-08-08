using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M200Attachments : Attachments
{
    [Header("Barrel")]
    public GameObject barrelAttachment;
    public Transform barrelAttachmentPos;

    [Header("Sight")]
    public GameObject sightAttachment;
    public Transform sightAttachmentPos;



    AudioClip normalShot;
    public AudioClip suppressedShotSound;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        if (barrelAttachment == null)
        {
            Destroy(barrelAttachment);
            GameObject newAttachment = Instantiate(attachment, barrelAttachmentPos);
            switch (gun)
            {
                case Gun.M4:
                    newAttachment.transform.localPosition = attachment.GetComponent<BarrelAttachment>().M4Position;
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


    public override void AttachSight(GameObject attachment, GameObject button)
    {
        if (sightAttachment == null)
        {

            Destroy(sightAttachment);
            GameObject newAttachment = Instantiate(attachment, sightAttachmentPos);
            newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().M4position;
            newAttachment.transform.rotation = sightAttachmentPos.rotation;
            sightAttachment = newAttachment;

            switch (newAttachment.GetComponent<SightAttachment>().sightType)
            {
                case SightAttachment.SightType.ACOG:
                    player.GetComponent<MouseLook>().acog = true;
                    break;
                case SightAttachment.SightType.HOLO:
                    player.GetComponent<MouseLook>().holo = true;
                    break;
            }

            switch (gun)
            {
                case Gun.M200:
                    newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().m200Position;
                    break;
            }

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
        currentGun.isSuppressed = false;
    }

    public override void UnequipSight()
    {
        player.GetComponent<MouseLook>().holo = false;
        player.GetComponent<MouseLook>().acog = false;
    }

}
