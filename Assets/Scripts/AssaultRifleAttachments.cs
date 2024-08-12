using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class AssaultRifleAttachments : Attachments
{



    [Header("Barrel")]
    public GameObject barrelAttachment;
    public Transform barrelAttachmentPos;

    [Header("Sight")]
    public GameObject sightAttachment;
    public Transform sightAttachmentPos;

    [Header("Underbarrel")]
    public GameObject underbarrelAttachment;
    public Transform underbarrelAttachmentPos;

    public GameObject[] ironSights;

    float normalRecoilX;
    float normalRecoilY;

    public AudioClip suppressedSound;


    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        if(barrelAttachment == null)
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
            currentGun.isSuppressed = true;
            currentGun.audioRange /= 2;
            currentGun.shotSound = suppressedSound;
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
                case Gun.M4:
                    newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().M4position;
                    break;
                case Gun.SCORPION:
                    newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().scorpionPosition;
                    break;

            }
            
            newAttachment.transform.rotation = sightAttachmentPos.rotation;
            sightAttachment = newAttachment;

            if (ironSights.Length > 0)
            {
                ironSights[0].SetActive(false);
                ironSights[1].transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            InventoryManager.Instance.attachmentsList.Remove(button);
            Destroy(button);
        }

    }

    public override void AttachUnderBarrel(GameObject attachment, GameObject button)
    {
        if(underbarrelAttachment == null)
        {
            shootScript.recoilY = shootScript.normalRecoilY;
            shootScript.recoilX = shootScript.normalRecoilX;

            shootScript.recoil.recoilY = shootScript.normalRecoilY;
            shootScript.recoil.recoilX = shootScript.normalRecoilX;

            Destroy(underbarrelAttachment);
            GameObject newAttachment = Instantiate(attachment, underbarrelAttachmentPos);
            newAttachment.transform.localPosition = attachment.GetComponent<UnderbarrelAttachment>().M4Position;
            newAttachment.transform.localRotation = Quaternion.Euler(0, 0, 0);
            underbarrelAttachment = newAttachment;

            shootScript.recoilY *= attachment.GetComponent<UnderbarrelAttachment>().verticalRecoilReduction;
            shootScript.recoilX *= attachment.GetComponent<UnderbarrelAttachment>().horizontalRecoilReduction;

            shootScript.recoil.recoilY *= attachment.GetComponent<UnderbarrelAttachment>().verticalRecoilReduction;
            shootScript.recoil.recoilX *= attachment.GetComponent<UnderbarrelAttachment>().horizontalRecoilReduction;

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

    public override void UnequipSight()
    {
        player.GetComponent<MouseLook>().acog = false;
        player.GetComponent<MouseLook>().holo = false;

        if (ironSights.Length > 0)
        {
            ironSights[0].SetActive(true);
            ironSights[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void UnequipUnderBarrel()
    {
        Shoot currentGun = transform.GetChild(0).GetComponent<Shoot>();
        shootScript.recoilY = shootScript.normalRecoilY;
        shootScript.recoilX = shootScript.normalRecoilX;

        shootScript.recoil.recoilY = shootScript.normalRecoilY;
        shootScript.recoil.recoilX = shootScript.normalRecoilX;

    }

}
