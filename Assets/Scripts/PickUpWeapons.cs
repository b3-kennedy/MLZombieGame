using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapons : MonoBehaviour
{

    public Transform weaponPos;
    List<GameObject> guns = new List<GameObject>();
    GameObject currentActiveGun;


    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.CompareTag("Gun"))
                {

                    PickUpGun(hit.collider.gameObject);
                    
                }
                else if (hit.collider.CompareTag("Mag"))
                {

                    PickUpMag(hit.collider.gameObject);
                    

                }
                else if (hit.collider.CompareTag("Attachment"))
                {
                    PickUpAttachment(hit.collider.gameObject);
                }
            }

        }

        SwitchWeapon();

    }


    void SwitchWeapon()
    {
        if(weaponPos.childCount > 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                
                foreach (var gun in guns)
                {
                    if (gun.activeSelf)
                    {
                        gun.SetActive(false);
                    }
                    else
                    {
                        gun.SetActive(true);
                        currentActiveGun = gun;
                        InventoryManager.Instance.activeGun = gun;
                        UpdateHud();
                        UpdateRecoilScript();

                    }
                }

                InventoryManager.Instance.OnOpenInventory();
            }
        }

    }

    void UpdateHud()
    {
        Shoot shootScript = currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>();
        if (shootScript.gunType == Shoot.GunType.ASSAULT_RIFLE)
        {
            GetComponent<HUDManager>().UpdateAmmoText(shootScript.currentAmmo, weaponPos.GetComponent<AmmoHolder>().arMags);
        }
        else if (shootScript.gunType == Shoot.GunType.PISTOL)
        {
            GetComponent<HUDManager>().UpdateAmmoText(shootScript.currentAmmo, weaponPos.GetComponent<AmmoHolder>().pistolMags);
        }
        else if(shootScript.gunType == Shoot.GunType.SHOTGUN)
        {
            GetComponent<HUDManager>().UpdateAmmoText(shootScript.currentAmmo, weaponPos.GetComponent<AmmoHolder>().shotgunShells);
        }
        else if (shootScript.gunType == Shoot.GunType.SMG)
        {
            GetComponent<HUDManager>().UpdateAmmoText(shootScript.currentAmmo, weaponPos.GetComponent<AmmoHolder>().smgMags);
        }
        GetComponent<HUDManager>().UpdateGunText(shootScript.gunName);
    }

    void UpdateRecoilScript()
    {
        Recoil.Instance.recoilX = currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().recoilX;
        Recoil.Instance.recoilY = currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().recoilY;
        Recoil.Instance.recoilZ = currentActiveGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().recoilZ;
    }

    void PickUpAttachment(GameObject attachment)
    {
        InventoryManager.Instance.OnPickUpItem(attachment, currentActiveGun);
    }

    void PickUpMag(GameObject mag)
    {
        MagazineType.MagType type = mag.GetComponent<MagazineType>().magType;
        switch (type)
        {
            case (MagazineType.MagType.ASSAULT_RIFLE):
                weaponPos.GetComponent<AmmoHolder>().arMags++;
                break;
            case (MagazineType.MagType.PISTOL):
                weaponPos.GetComponent<AmmoHolder>().pistolMags++;
                break;
            case (MagazineType.MagType.SHOTGUN):
                weaponPos.GetComponent<AmmoHolder>().shotgunShells++;
                break;
            case (MagazineType.MagType.SMG):
                weaponPos.GetComponent<AmmoHolder>().smgMags++;
                break;
        }

        if (weaponPos.childCount > 0)
        {
            GameObject currentGun = weaponPos.GetChild(0).gameObject;
            currentGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().UpdateMagCount();
            UpdateHud();
        }
        
        Destroy(mag);
    }

    void PickUpGun(GameObject gun)
    {
        if(weaponPos.childCount < 3)
        {
            Debug.Log("pickup");
            WeaponPickupObject wpo = gun.GetComponent<WeaponPickupObject>();

            

            if (weaponPos.childCount > 1)
            {
                for (int i = 0; i < weaponPos.childCount; i++)
                {
                    if (weaponPos.GetChild(i).gameObject.activeSelf)
                    {
                        GameObject currentGun = weaponPos.GetChild(i).gameObject;
                        Transform attachmentParent = currentGun.transform.GetChild(0).GetChild(0).GetChild(0);

                        if(attachmentParent.GetChild(0).childCount > 0)
                        {
                            InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(0).GetChild(0).gameObject, currentGun);
                        }
                        if(attachmentParent.childCount > 1)
                        {
                            if (attachmentParent.GetChild(1).childCount > 0)
                            {
                                InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(1).GetChild(0).gameObject, currentGun);
                            }
                        }
                        if(attachmentParent.childCount > 2)
                        {
                            if (attachmentParent.GetChild(1).childCount > 0)
                            {
                                InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(2).GetChild(0).gameObject, currentGun);
                            }
                        }



                        GameObject droppedGun = Instantiate(currentGun.GetComponent<WeaponDropObject>().gunObject, gun.transform.position, gun.transform.rotation);
                        droppedGun.GetComponent<WeaponPickupObject>().currentAmmo = currentGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().currentAmmo;
                        Destroy(currentGun);
                        guns.Remove(currentGun);
                    }
                }

            }



            GameObject newGun = Instantiate(wpo.gunObject, weaponPos);
            guns.Add(newGun);
            newGun.transform.SetAsFirstSibling();
            Shoot newGunShootScript = newGun.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Shoot>();
            newGunShootScript.currentAmmo = wpo.currentAmmo;
            newGun.transform.localPosition = Vector3.zero;

            

            if (weaponPos.childCount > 0)
            {
                weaponPos.GetChild(0).gameObject.SetActive(true);
                currentActiveGun = weaponPos.GetChild(0).gameObject;
                InventoryManager.Instance.activeGun = currentActiveGun;
                if (weaponPos.childCount > 1)
                {
                    weaponPos.GetChild(1).gameObject.SetActive(false);
                }
                
            }

            UpdateRecoilScript();
            UpdateHud();

            Destroy(gun);
            InventoryManager.Instance.OnPickUpWeapon(newGun);
        }

    }
}
