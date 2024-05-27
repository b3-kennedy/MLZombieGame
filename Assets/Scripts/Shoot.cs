using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    
    public enum ShootType {SINGLE, AUTO};
    public enum GunType {PISTOL, ASSAULT_RIFLE};

    public GameObject muzzleFlashSpawner;

    [Header("Audio Settings")]
    public AudioClip shotSound;
    public AudioClip emptySound;
    public Transform audioSpawn;

    [Header("Gun Settings")]
    public ShootType shootType;
    public GunType gunType;


    public float fireRate;

    float nextFire = 0f;

    [Header("Ammo and Reload Settings")]
    public float maxAmmo;

    public float currentAmmo;
    public float currentMags;

    public float reloadTime;

    public bool reload;
    float reloadTimer;

    public float damage;
    public float headshotDamage;
    
    

    [HideInInspector] public Recoil recoil;

    [Header("Recoil Settings")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [HideInInspector] public float normalRecoilX;
    [HideInInspector] public float normalRecoilY;
    [HideInInspector] public float normalRecoilZ;

    public float snap;
    public float returnSpeed;
    Animator anim;
    AmmoHolder ammoHolder;

    public bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        ammoHolder = transform.parent.parent.parent.GetComponent<AmmoHolder>();
        //currentAmmo = maxAmmo;

        UpdateMagCount();

        anim = transform.parent.parent.GetComponent<Animator>();

        recoil = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Recoil>();

        Debug.Log(recoil);

        recoil.recoilX = recoilX;
        recoil.recoilY = recoilY;
        recoil.recoilZ = recoilZ;

        recoil.snap = snap;
        recoil.returnSpeed = returnSpeed;

        normalRecoilX = recoilX;
        normalRecoilY = recoilY;
        normalRecoilZ = recoilZ;


    }

    public void UpdateMagCount()
    {
        switch (gunType)
        {
            case GunType.PISTOL:
                currentMags = ammoHolder.pistolMags;
                break;
            case GunType.ASSAULT_RIFLE:
                currentMags = ammoHolder.arMags;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!InventoryManager.Instance.inventory.activeSelf)
        {
            if (currentAmmo > 0)
            {
                if (shootType == ShootType.AUTO)
                {
                    if (Input.GetButton("Fire1") && Time.time >= nextFire)
                    {
                        nextFire = Time.time + 1f / fireRate;
                        Fire();
                        recoil.RecoilFire();
                    }
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        anim.SetBool("shoot", false);
                    }
                }
                else if (shootType == ShootType.SINGLE)
                {
                    if (Input.GetButtonDown("Fire1") && Time.time >= nextFire)
                    {
                        nextFire = Time.time + 1f / fireRate;
                        Fire();
                        recoil.RecoilFire();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    AudioSource.PlayClipAtPoint(emptySound, audioSpawn.position);
                }
                anim.SetBool("shoot", false);
            }
        }


        if(Input.GetKeyDown(KeyCode.R) && gunType == GunType.ASSAULT_RIFLE && ammoHolder.arMags > 0)
        {
            reload = true;
        }
        else if(Input.GetKeyDown(KeyCode.R) && gunType == GunType.PISTOL && ammoHolder.pistolMags > 0)
        {
            reload = true;
        }

        Reload();
    }

    void Reload()
    {
        if (reload)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= reloadTime)
            {
                currentAmmo = maxAmmo;
                reloadTimer = 0;
                reload = false;
                switch (gunType)
                {
                    case GunType.PISTOL:
                        ammoHolder.pistolMags -= 1;
                        break;
                    case GunType.ASSAULT_RIFLE:
                        ammoHolder.arMags -= 1;
                        break;

                }
                UpdateMagCount();
            }
        }
    }

    void Fire()
    {

        GameObject muzzleFlash = Instantiate(muzzleFlashSpawner, audioSpawn.position, audioSpawn.transform.rotation);
        muzzleFlash.transform.SetParent(audioSpawn.transform);

        AudioSource.PlayClipAtPoint(shotSound, audioSpawn.position);
        currentAmmo -= 1;
        if(currentAmmo <= 0) 
        {
            AudioSource.PlayClipAtPoint(emptySound, audioSpawn.position);
        }
        anim.SetBool("shoot", true);
        //Debug.Log("shoot");
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000))
        {
            if (hit.collider.transform.root.GetComponent<Health>())
            {
                if (hit.collider.CompareTag("Head"))
                {
                    hit.collider.transform.root.GetComponent<Health>().TakeDamage(headshotDamage);
                }
                else if (hit.collider.CompareTag("Body"))
                {
                    hit.collider.transform.root.GetComponent<Health>().TakeDamage(damage);
                }
            }

        }
    }
}
