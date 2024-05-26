using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Shoot shootScript;

    private void Start()
    {
        shootScript = transform.GetChild(0).GetComponent<Shoot>();
    }

    private void Update()
    {




    }


    public override void AttachBarrel(GameObject attachment, GameObject button)
    {
        Destroy(barrelAttachment);
        GameObject newAttachment = Instantiate(attachment, barrelAttachmentPos);
        switch (gun)
        {
            case Gun.M4:
                newAttachment.transform.localPosition = attachment.GetComponent<BarrelAttachment>().M4Position;
                break;
        }
        
        newAttachment.transform.localRotation = Quaternion.Euler(0,0,0);
        barrelAttachment = newAttachment;
        Destroy(button);
    }

    public override void AttachSight(GameObject attachment, GameObject button)
    {
        Destroy(sightAttachment);
        GameObject newAttachment = Instantiate(attachment, sightAttachmentPos);
        newAttachment.transform.localPosition = attachment.GetComponent<SightAttachment>().M4position;
        newAttachment.transform.rotation = sightAttachmentPos.rotation;
        sightAttachment = newAttachment;

        if(ironSights.Length > 0)
        {
            ironSights[0].SetActive(false);
            ironSights[1].transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        Destroy(button);
    }

    public override void AttachUnderBarrel(GameObject attachment, GameObject button)
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
        Destroy(button);
    }

    public override void UnequipSight()
    {
        if (ironSights.Length > 0)
        {
            ironSights[0].SetActive(true);
            ironSights[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

}