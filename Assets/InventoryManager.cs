using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


    List<GameObject> createdSlots = new List<GameObject>();

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
            if(slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.SIGHT)
            {
                GameObject sight = Instantiate(sightSlot, attachmesntSlotSpawns[slotNumber].transform);
                createdSlots.Add(sight);
                
            }
            else if(slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.BARREL)
            {
                GameObject barrel = Instantiate(barrelSlot, attachmesntSlotSpawns[slotNumber].transform);
                createdSlots.Add(barrel);
            }
            else if(slot.GetComponent<AttachmentSlotType>().attachmentType == AttachmentSlotType.AttachmentType.UNDERBARREL)
            {
                GameObject underbarrel = Instantiate(underbarrelSlot, attachmesntSlotSpawns[slotNumber].transform);
                createdSlots.Add(underbarrel);
            }
        }
    }

    public void OnPickUpItem(GameObject item)
    {
        Debug.Log("picked up: " + item.name);
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
                gun1Slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponPos.GetChild(i).gameObject.name;
                GunInventoryUI(weaponPos.GetChild(i).gameObject, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !inventory.activeSelf)
        {
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            OnOpenInventory();

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && inventory.activeSelf)
        {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
