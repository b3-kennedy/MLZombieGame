using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachments : MonoBehaviour
{

    [HideInInspector] public GameObject player;
    [HideInInspector] public Shoot shootScript;
    [HideInInspector] public AudioClip normalSound;


    private void Start()
    {
        shootScript = transform.GetChild(0).GetComponent<Shoot>();
        player = transform.parent.parent.GetComponent<AmmoHolder>().player;
        normalSound = shootScript.shotSound;
    }

    public enum Gun { M4, GLOCK, MP7, M240, SCAR, SCORPION, M200, TAR21};
    public Gun gun;
    public virtual void AttachBarrel(GameObject attachment, GameObject button) { }
    public virtual void AttachSight(GameObject attachment, GameObject button) { }

    public virtual void AttachUnderBarrel(GameObject attachment, GameObject button) { }

    public virtual void UnequipSight() { }

    public virtual void UnequipBarrel() { }

    public virtual void UnequipUnderBarrel() { }

}
