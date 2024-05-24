using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : MonoBehaviour
{

    public Transform weaponPos;
    List<GameObject> guns = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10))
        {
            if (hit.collider.CompareTag("Gun"))
            {
                Debug.Log("hit gun");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpGun(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("Mag"))
            {
                Debug.Log("hit mag");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpMag(hit.collider.gameObject);
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
                    }
                }
            }
        }

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
        }

        if (weaponPos.childCount > 0)
        {
            GameObject currentGun = weaponPos.GetChild(0).gameObject;
            currentGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().UpdateMagCount();
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
                Debug.Log("2");
                for (int i = 0; i < weaponPos.childCount; i++)
                {
                    if (weaponPos.GetChild(i).gameObject.activeSelf)
                    {
                        GameObject currentGun = weaponPos.GetChild(i).gameObject;
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
            Debug.Log(newGunShootScript.currentAmmo);
            newGun.transform.localPosition = Vector3.zero;

            if(weaponPos.childCount > 0)
            {
                weaponPos.GetChild(0).gameObject.SetActive(true);
                if(weaponPos.childCount > 1)
                {
                    weaponPos.GetChild(1).gameObject.SetActive(false);
                }
                
            }


            Destroy(gun);
        }

    }
}
