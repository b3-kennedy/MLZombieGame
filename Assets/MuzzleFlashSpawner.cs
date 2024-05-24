using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashSpawner : MonoBehaviour
{
    public GameObject[] muzzleFlashes;


    private void Start()
    {
        int randomNum = Random.Range(0, muzzleFlashes.Length);
        GameObject flash = Instantiate(muzzleFlashes[randomNum], transform.position, Quaternion.identity);
        flash.transform.SetParent(transform);
        flash.transform.localScale = flash.transform.localScale / 3;
        flash.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
