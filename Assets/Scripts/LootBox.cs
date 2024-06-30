using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListShuffle : MonoBehaviour
{
    public static List<T> Execute<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}

[System.Serializable]
public class LootList
{
    public List<Loot> gunList;
    public List<Loot> ammoList;
    public List<Loot> attachmentList;
}

[System.Serializable]
public class ItemChance
{
    public string itemName;
    public enum ItemType {GUN, AMMO, ATTACHMENT};
    public ItemType type;
    public int chance;
}

public class LootBox : MonoBehaviour
{
    public LootList loot;
    public ItemChance[] chances;
    public int numberOfItems;
    public List<Loot> items = new List<Loot>();

    // Start is called before the first frame update
    void Start()
    {
        //Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Loot AddGun()
    {
        int randomNum = Random.Range(1, 101);
        List<Loot> randomList = ListShuffle.Execute(loot.gunList);
        foreach (var item in randomList)
        {
            if(randomNum <= item.dropChance)
            {
                return item;
            }
        }
        return AddGun();
    }

    Loot AddAmmo()
    {
        int randomNum = Random.Range(1, 101);
        List<Loot> randomList = ListShuffle.Execute(loot.ammoList);
        foreach (var item in randomList)
        {
            if (randomNum <= item.dropChance)
            {
                return item;
            }
        }
        return AddAmmo();
    }

    Loot AddAttachment()
    {
        int randomNum = Random.Range(1, 101);
        List<Loot> randomList = ListShuffle.Execute(loot.attachmentList);
        foreach (var item in randomList)
        {
            if (randomNum <= item.dropChance)
            {
                return item;
            }
        }
        return AddAttachment();
    }

    public void Open()
    {
        int randomItemChance = Random.Range(1, 101);
        foreach (var item in chances)
        {
            if (randomItemChance <= item.chance)
            {
                if(item.type == ItemChance.ItemType.GUN && items.Count < numberOfItems)
                {
                    items.Add(AddGun());
                }
                else if(item.type == ItemChance.ItemType.AMMO && items.Count < numberOfItems)
                {
                    items.Add(AddAmmo());
                }
                else if (item.type == ItemChance.ItemType.ATTACHMENT && items.Count < numberOfItems)
                {
                    items.Add(AddAttachment());
                }
            }
        }

        if(items.Count < numberOfItems)
        {
            Open();
        }
        else
        {
            Destroy(gameObject);
            int index = 0;
            foreach (var item in items)
            {
                Debug.Log(item.name);
                GameObject itemObj = Instantiate(item.lootItem, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                itemObj.GetComponent<Rigidbody>().AddForce(transform.GetChild(index).up * 1, ForceMode.Impulse);
                index++;
            }
            
        }

        items.Clear();
        

    }
}
