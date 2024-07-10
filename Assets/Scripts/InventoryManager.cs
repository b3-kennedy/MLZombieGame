using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject inventory;
    public Transform weaponPos;
    public GameObject gun1Slot;

    public GameObject[] attachmesntSlotSpawns;

    public GameObject sightSlot;
    public GameObject barrelSlot;
    public GameObject underbarrelSlot;

    public Transform itemParent;

    public List<GameObject> attachmentsList;

    [HideInInspector] public GameObject activeGun;

    List<GameObject> createdSlots = new List<GameObject>();

    public GameObject sight;
    public GameObject barrel;
    public GameObject underbarrel;

    public Image throwableSlot;

    public TextMeshProUGUI pistolMagText;
    public TextMeshProUGUI arMagText;
    public TextMeshProUGUI smgMagText;
    public TextMeshProUGUI lmgMagText;
    public TextMeshProUGUI sniperMagText;
    public TextMeshProUGUI shotgunMagText;


    private void Awake()
    {
        Instance = this;
        inventory.SetActive(false);
    }

    public void OnPickUpWeapon(GameObject weapon)
    {

        //for (int i = 0; i < createdSlots.Count; i++)
        //{
        //    Destroy(createdSlots[i]);
        //}
        //createdSlots.Clear();

        //Debug.Log("picked up: " + weapon.name);
        //if(weaponPos.childCount == 1)
        //{
        //    gun1Slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapon.name;
        //    GunInventoryUI(weapon, 0);
        //}
        //else if(weaponPos.childCount > 1)
        //{
        //    gun1Slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponPos.GetChild(0).gameObject.name;
        //    Debug.Log(weaponPos.GetChild(0).gameObject);
        //    Debug.Log(weaponPos.GetChild(0).gameObject);
        //    GunInventoryUI(weaponPos.GetChild(0).gameObject, 0);
        //    GunInventoryUI(weaponPos.GetChild(1).gameObject, 1);
        //}

    }

    public void UpdateAmmoInInventory()
    {
        AmmoHolder holder = weaponPos.GetComponent<AmmoHolder>();
        pistolMagText.text = holder.pistolMags.ToString();
        arMagText.text = holder.arMags.ToString();
        smgMagText.text = holder.smgMags.ToString();
        lmgMagText.text = holder.lmgMags.ToString();
        sniperMagText.text = holder.sniperMags.ToString();
        shotgunMagText.text = holder.shotgunShells.ToString();

    }

    void GunInventoryUI(GameObject gun, int slotNumber)
    {
        Transform attachments = gun.transform.GetChild(0).GetChild(0).GetChild(0);
        List<GameObject> attachmentSlots = new List<GameObject>();
        for (int i = 0; i < attachments.childCount; i++)
        {
            attachmentSlots.Add(attachments.GetChild(i).gameObject);
        }

        foreach (var slot in attachmentSlots)
        {
            if (slot.GetComponent<AttachmentSlotType>())
            {
                if (slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.SIGHT)
                {
                    sight = Instantiate(sightSlot, attachmesntSlotSpawns[slotNumber].transform);
                    createdSlots.Add(sight);
                    if (slot.transform.childCount > 0)
                    {
                        sight.GetComponent<AttachmentSlot>().attachmentUI = slot.transform.GetChild(0).GetComponent<Attachment>().attachmentUI;
                        sight.GetComponent<AttachmentSlot>().attachmentObj = slot.transform.GetChild(0).gameObject;
                        sight.GetComponent<Image>().color = Color.green;
                    }

                }
                else if (slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.BARREL)
                {
                    barrel = Instantiate(barrelSlot, attachmesntSlotSpawns[slotNumber].transform);
                    createdSlots.Add(barrel);
                    if (slot.transform.childCount > 0)
                    {
                        barrel.GetComponent<AttachmentSlot>().attachmentUI = slot.transform.GetChild(0).GetComponent<Attachment>().attachmentUI;
                        barrel.GetComponent<AttachmentSlot>().attachmentObj = slot.transform.GetChild(0).gameObject;
                        barrel.GetComponent<Image>().color = Color.green;
                    }
                }
                else if (slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.UNDERBARREL)
                {
                    underbarrel = Instantiate(underbarrelSlot, attachmesntSlotSpawns[slotNumber].transform);
                    createdSlots.Add(underbarrel);
                    if (slot.transform.childCount > 0)
                    {
                        underbarrel.GetComponent<AttachmentSlot>().attachmentUI = slot.transform.GetChild(0).GetComponent<Attachment>().attachmentUI;
                        underbarrel.GetComponent<AttachmentSlot>().attachmentObj = slot.transform.GetChild(0).gameObject;
                        underbarrel.GetComponent<Image>().color = Color.green;
                    }
                }
            }

        }
        List<GameObject> tempList = attachmentsList;
        foreach (var attachment in tempList)
        {
            attachment.GetComponent<Button>().onClick.RemoveAllListeners();
            switch (attachment.GetComponent<AttachmentItem>().attachmentType)
            {
                case AttachmentSlotType.AttachmentType.SIGHT:
                    
                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachSight(attachment.GetComponent<AttachmentItem>().item, attachment);
                    });
                    Debug.Log(attachment.GetComponent<Button>().onClick);
                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });
                    break;
                case AttachmentSlotType.AttachmentType.BARREL:
                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachBarrel(attachment.GetComponent<AttachmentItem>().item, attachment);
                    });

                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });
                    break;
                case AttachmentSlotType.AttachmentType.UNDERBARREL:
                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachUnderBarrel(attachment.GetComponent<AttachmentItem>().item, attachment);
                    });

                    attachment.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });
                    break;
            }
        }
        

    }

    public void OnUnequipItem(GameObject itemUI, GameObject itemGameObject, Image slot)
    {
        if(itemUI != null)
        {
            GameObject attachmentUI = Instantiate(itemUI, InventoryManager.Instance.itemParent);

            switch (attachmentUI.GetComponent<AttachmentItem>().attachmentType)
            {
                case AttachmentSlotType.AttachmentType.SIGHT:
                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachSight(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
                    });

                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });

                    Debug.Log(activeGun);
                    activeGun.transform.GetChild(0).GetComponent<Attachments>().UnequipSight();

                    break;
                case AttachmentSlotType.AttachmentType.BARREL:
                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachBarrel(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
                    });

                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });

                    activeGun.transform.GetChild(0).GetComponent<Attachments>().UnequipBarrel();

                    break;
                case AttachmentSlotType.AttachmentType .UNDERBARREL:
                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        activeGun.transform.GetChild(0).GetComponent<Attachments>().AttachUnderBarrel(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
                    });

                    attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
                        OnOpenInventory();
                    });

                    activeGun.transform.GetChild(0).GetComponent<Attachments>().UnequipUnderBarrel();

                    break;
            }
        }

        if(itemGameObject != null)
        {
            Destroy(itemGameObject);
        }
        
        slot.color = Color.white;
    }

    public void PickUpThrowable(GameObject throwable)
    {
        if (throwable.GetComponent<Throwable>())
        {
            GameObject ui = Instantiate(throwable.GetComponent<Throwable>().uiElement, itemParent);
            ui.GetComponent<Button>().onClick.AddListener(delegate { EquipThrowable(ui); });
            ui.GetComponent<Button>().onClick.AddListener(delegate { OnOpenInventory(); });

        }
    }

    public void EquipThrowable(GameObject ui)
    {
        if(throwableSlot.GetComponent<ThrowableSlot>().item == null)
        {
            Destroy(ui);
            throwableSlot.color = Color.green;
            throwableSlot.GetComponent<ThrowableSlot>().item = ui.GetComponent<BottleItem>().bottle;
        }

    }

    public void OnPickUpItem(GameObject attachment, GameObject currentActiveGun)
    {
        if (attachment.GetComponent<ItemPickupObject>())
        {
            GameObject attachmentUI = Instantiate(attachment.GetComponent<ItemPickupObject>().UIElement, InventoryManager.Instance.itemParent);
            attachmentsList.Add(attachmentUI);
            activeGun = currentActiveGun;
        }
        else if (attachment.GetComponent<Attachment>())
        {
            GameObject attachmentUI = Instantiate(attachment.GetComponent<Attachment>().attachmentUI, InventoryManager.Instance.itemParent);
            attachmentsList.Add(attachmentUI);
            activeGun = currentActiveGun;
        }

        

        //if(currentActiveGun != null)
        //{
        //    switch (attachment.GetComponent<ItemPickupObject>().attachmentType)
        //    {
        //        case AttachmentSlotType.AttachmentType.SIGHT:
        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                currentActiveGun.transform.GetChild(0).GetComponent<Attachments>().AttachSight(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
        //            });

        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                OnOpenInventory();
        //            });
        //            break;
        //        case AttachmentSlotType.AttachmentType.BARREL:
        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                currentActiveGun.transform.GetChild(0).GetComponent<Attachments>().AttachBarrel(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
        //            });

        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                OnOpenInventory();
        //            });
        //            break;
        //        case AttachmentSlotType.AttachmentType.UNDERBARREL:
        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                currentActiveGun.transform.GetChild(0).GetComponent<Attachments>().AttachUnderBarrel(attachmentUI.GetComponent<AttachmentItem>().item, attachmentUI);
        //            });

        //            attachmentUI.GetComponent<Button>().onClick.AddListener(delegate {
        //                OnOpenInventory();
        //            });
        //            break;
        //    }
            
        //}


        Destroy(attachment);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnOpenInventory()
    {

        for (int i = 0; i < createdSlots.Count; i++)
        {
            Destroy(createdSlots[i]);
        }
        createdSlots.Clear();

        for (int i = 0; i < weaponPos.childCount; i++) 
        {
            if (weaponPos.GetChild(i).gameObject.activeSelf)
            {
                gun1Slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = activeGun.transform.GetChild(0).GetChild(0).GetComponent<Shoot>().gunName;
                GunInventoryUI(weaponPos.GetChild(i).gameObject, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance != null && GameManager.Instance.gameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !inventory.activeSelf)
        {
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            OnOpenInventory();

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && inventory.activeSelf)
        {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
