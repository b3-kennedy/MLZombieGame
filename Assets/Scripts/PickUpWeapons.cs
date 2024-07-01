using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapons : MonoBehaviour
{

    public Transform weaponPos;
    List<GameObject> guns = new List<GameObject>();
    GameObject currentActiveGun;
    ThrowableSlot throwableSlot;
    public Transform throwPoint;
    public float throwForce = 10;
    public AudioClip openBox;

    private void Start()
    {
        throwableSlot = InventoryManager.Instance.throwableSlot.GetComponent<ThrowableSlot>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 3))
        {
            if (hit.collider.CompareTag("Gun"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpGun(hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up " + hit.collider.GetComponent<WeaponPickupObject>().gunObject.name);
                HUDManager.Instance.ShowInteractPrompt();
            }
            else if (hit.collider.CompareTag("Mag"))
            {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpMag(hit.collider.gameObject);
                }
                MagazineType type = hit.collider.GetComponent<MagazineType>();
                switch (type.magType)
                {
                    case (MagazineType.MagType.PISTOL):
                        HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up pistol magazine");
                        break;
                    case (MagazineType.MagType.ASSAULT_RIFLE):
                        HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up assault rifle magazine");
                        break;
                    case (MagazineType.MagType.SMG):
                        HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up sub machine gun magazine");
                        break;
                    case (MagazineType.MagType.LMG):
                        HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up light machine gun magazine");
                        break;
                    case (MagazineType.MagType.SHOTGUN):
                        HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up shotgun shells");
                        break;
                }
                
                HUDManager.Instance.ShowInteractPrompt();
            }
            else if (hit.collider.CompareTag("Attachment"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpAttachment(hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up " + hit.collider.GetComponent<ItemPickupObject>().itemName);
                HUDManager.Instance.ShowInteractPrompt();

            }
            else if (hit.collider.CompareTag("LootBox"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    AudioSource.PlayClipAtPoint(openBox, hit.collider.transform.position);
                    hit.collider.GetComponent<LootBox>().Open();
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to open");
                HUDManager.Instance.ShowInteractPrompt();
                

            }
            else if (hit.collider.CompareTag("Throwable"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpThrowable(hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up " + hit.collider.GetComponent<Throwable>().itemName);
                HUDManager.Instance.ShowInteractPrompt();

            }
            else if (hit.collider.CompareTag("RedKey"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.hasRedKey = true;
                    Destroy(hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up red key");
                HUDManager.Instance.ShowInteractPrompt();
            }
            else if (hit.collider.CompareTag("BlueKey"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.hasBlueKey = true;
                    Destroy(hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up blue key");
                HUDManager.Instance.ShowInteractPrompt();
            }
            else if (hit.collider.CompareTag("GreenKey"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.hasGreenKey = true;
                    Destroy (hit.collider.gameObject);
                }
                HUDManager.Instance.UpdateInteractPrompt("Press 'F' to pick up green key");
                HUDManager.Instance.ShowInteractPrompt();
            }
            else if (hit.collider.CompareTag("KeyWall"))
            {
                if(GameManager.Instance.hasRedKey && GameManager.Instance.hasBlueKey && GameManager.Instance.hasGreenKey)
                {
                    HUDManager.Instance.UpdateInteractPrompt("Press 'F' to open door");
                    HUDManager.Instance.ShowInteractPrompt();
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        hit.collider.gameObject.SetActive(false);
                    }
                }
                else
                {
                    HUDManager.Instance.UpdateInteractPrompt("Find all keys to open door");
                    HUDManager.Instance.ShowInteractPrompt();
                }
            }
            else
            {
                HUDManager.Instance.HideInteractPrompt();
                
            }
        }
        else
        {
            HUDManager.Instance.HideInteractPrompt();
        }

        SwitchWeapon();

    }

    void PickUpThrowable(GameObject obj)
    {
        InventoryManager.Instance.PickUpThrowable(obj);
        Destroy(obj);
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

        if(Input.GetKeyUp(KeyCode.G) && throwableSlot.item != null)
        {
            GameObject throwableItem = Instantiate(throwableSlot.item, throwPoint.position, Quaternion.identity);
            if (throwableItem.GetComponent<SmokeGrenade>())
            {
                throwableItem.GetComponent<SmokeGrenade>().isActive = true;
            }
            if (throwableItem.GetComponent<Rotate>())
            {
                throwableItem.GetComponent<Rotate>().enabled = true;
            }
            if(throwableItem.GetComponent<Bottle>())
            {
                throwableItem.GetComponent<Bottle>().enabled = true;
            }
            throwableItem.GetComponent<Rigidbody>().AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            throwableSlot.ItemThrown();
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
        else if(shootScript.gunType == Shoot.GunType.LMG)
        {
            GetComponent<HUDManager>().UpdateAmmoText(shootScript.currentAmmo, weaponPos.GetComponent<AmmoHolder>().lmgMags);
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
            case (MagazineType.MagType.LMG):
                weaponPos.GetComponent<AmmoHolder>().lmgMags++;
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
                        if(attachmentParent.childCount > 0)
                        {
                            if (attachmentParent.GetChild(0).childCount > 0)
                            {
                                InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(0).GetChild(0).gameObject, currentGun);
                            }
                            if (attachmentParent.childCount > 1)
                            {
                                if (attachmentParent.GetChild(1).childCount > 0)
                                {
                                    InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(1).GetChild(0).gameObject, currentGun);
                                }
                            }
                            if (attachmentParent.childCount > 2)
                            {
                                if (attachmentParent.GetChild(1).childCount > 0)
                                {
                                    InventoryManager.Instance.OnPickUpItem(attachmentParent.GetChild(2).GetChild(0).gameObject, currentGun);
                                }
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
