using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : MonoBehaviour
{

    public Transform weaponPos;

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

        Debug.Log("pickup");
        WeaponPickupObject wpo = gun.GetComponent<WeaponPickupObject>();

        if(weaponPos.childCount > 0)
        {
            GameObject currentGun = weaponPos.GetChild(0).gameObject;
            GameObject droppedGun = Instantiate(currentGun.GetComponent<WeaponDropObject>().gunObject, gun.transform.position, gun.transform.rotation);
            droppedGun.GetComponent<WeaponPickupObject>().currentAmmo = currentGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().currentAmmo;
            Destroy(currentGun);
        }
        

        GameObject newGun = Instantiate(wpo.gunObject, weaponPos);
        Shoot newGunShootScript = newGun.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Shoot>();
        newGunShootScript.currentAmmo = wpo.currentAmmo;
        Debug.Log(newGunShootScript.currentAmmo);
        newGun.transform.localPosition = Vector3.zero;

        Destroy(gun);
    }
}
