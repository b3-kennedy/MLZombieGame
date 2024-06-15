using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject lootItem;
    public int dropChance;

    public Loot(GameObject lootItem, int dropChance)
    {
        this.lootItem = lootItem;
        this.dropChance = dropChance;
    }
}
