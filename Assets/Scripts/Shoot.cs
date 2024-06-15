using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Shoot : MonoBehaviour
{
    
    public enum ShootType {SINGLE, AUTO};
    public enum GunType {PISTOL, ASSAULT_RIFLE, SHOTGUN, SMG};

    public GameObject muzzleFlashSpawner;

    [Header("Audio Settings")]
    public AudioClip shotSound;
    public AudioClip emptySound;
    public Transform audioSpawn;
    public float audioRange;
    public LayerMask zombieLayer;

    [Header("Gun Settings")]
    public ShootType shootType;
    public GunType gunType;


    public float fireRate;

    float nextFire = 0f;

    [Header("Ammo and Reload Settings")]
    public int maxAmmo;

    public int currentAmmo;
    public int currentMags;

    public float reloadTime;

    public bool reload;
    float reloadTimer;

    public float damage;
    public float headshotDamage;

    [Header("Shotgun Settings")]
    public float pelletsPerShot;
    public float spreadAngle;
    public float range;
    public float verticalSpreadMultiplier;

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

    public string gunName;

    public GameObject testSphere;

    List<Vector3> shotgunHits = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        ammoHolder = transform.parent.parent.parent.GetComponent<AmmoHolder>();
        //currentAmmo = maxAmmo;

        UpdateMagCount();

        anim = transform.parent.parent.GetComponent<Animator>();

        recoil = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Recoil>();

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
            case GunType.SHOTGUN:
                currentMags = ammoHolder.shotgunShells;
                break;
            case GunType.SMG:
                currentMags = ammoHolder.smgMags;
                break;

        }

        
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.Instance != null && GameManager.Instance.gameOver)
        {
            return;
        }

        if (!InventoryManager.Instance.inventory.activeSelf && !reload)
        {
            if (currentAmmo > 0)
            {
                if(gunType != GunType.SHOTGUN)
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
                    if (shootType == ShootType.AUTO)
                    {
                        if (Input.GetButton("Fire1") && Time.time >= nextFire)
                        {
                            nextFire = Time.time + 1f / fireRate;
                            FireShotgun();
                            recoil.RecoilFire();
                            shotgunHits.Clear();
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
                            FireShotgun();
                            recoil.RecoilFire();
                            shotgunHits.Clear();
                        }
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

        if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.ASSAULT_RIFLE && ammoHolder.arMags > 0)
        {
            reload = true;
            anim.SetBool("reload", true);
        }
        else if(Input.GetKeyDown(KeyCode.R) && gunType == GunType.PISTOL && ammoHolder.pistolMags > 0)
        {
            anim.SetBool("reload", true);
            reload = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.SHOTGUN && ammoHolder.shotgunShells > 0)
        {
            anim.SetBool("reload", true);
            reload = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.SMG && ammoHolder.smgMags > 0)
        {
            anim.SetBool("reload", true);
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
                anim.SetBool("reload", false);
                switch (gunType)
                {
                    case GunType.PISTOL:
                        ammoHolder.pistolMags -= 1;
                        break;
                    case GunType.ASSAULT_RIFLE:
                        ammoHolder.arMags -= 1;
                        break;
                    case GunType.SHOTGUN:
                        ammoHolder.shotgunShells -= 1;
                        break;
                    case GunType.SMG:
                        ammoHolder.smgMags -= 1;
                        break;

                }
                UpdateMagCount();
                HUDManager.Instance.UpdateAmmoText(currentAmmo, currentMags);
            }
        }
    }


    void FireShotgun()
    {
        GameObject muzzleFlash = Instantiate(muzzleFlashSpawner, audioSpawn.position, audioSpawn.transform.rotation);
        muzzleFlash.transform.SetParent(audioSpawn.transform);

        AudioSource.PlayClipAtPoint(shotSound, audioSpawn.position);
        Vector3 shotPos = audioSpawn.position;
        currentAmmo -= 1;
        HUDManager.Instance.UpdateAmmoText(currentAmmo, currentMags);
        if (currentAmmo <= 0)
        {
            AudioSource.PlayClipAtPoint(emptySound, audioSpawn.position);
        }
        anim.SetBool("shoot", true);

        Vector3 baseDirection = Camera.main.transform.forward;

        for (int i = 0; i < pelletsPerShot; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, CalculateShootingDir(), out hit))
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


            //Instantiate(testSphere, hit.point, Quaternion.identity);

        }

        CheckAudioRange(shotPos);
        //Debug.Log("shoot");

    }

    Vector3 CalculateShootingDir()
    {
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * range;
        targetPos = new Vector3(targetPos.x + Random.Range(-spreadAngle, spreadAngle), targetPos.y + Random.Range(-spreadAngle, spreadAngle), targetPos.z + Random.Range(-spreadAngle, spreadAngle));

        Vector3 direction = targetPos - Camera.main.transform.position;
        return direction.normalized;
    }

    void Fire()
    {

        GameObject muzzleFlash = Instantiate(muzzleFlashSpawner, audioSpawn.position, audioSpawn.transform.rotation);
        muzzleFlash.transform.SetParent(audioSpawn.transform);

        AudioSource.PlayClipAtPoint(shotSound, audioSpawn.position);
        Vector3 shotPos = audioSpawn.position;
        currentAmmo -= 1;
        HUDManager.Instance.UpdateAmmoText(currentAmmo, currentMags);
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

        CheckAudioRange(shotPos);
    }

    void CheckAudioRange(Vector3 shotPos)
    {
        Collider[] cols = Physics.OverlapSphere(audioSpawn.position, audioRange, zombieLayer);

        foreach (var col in cols)
        {
            if (col.GetComponent<ZombiePatrolAI>())
            {
                col.GetComponent<ZombiePatrolAI>().HeardSound(shotPos);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(audioSpawn.position, audioRange);
        
    }
}
