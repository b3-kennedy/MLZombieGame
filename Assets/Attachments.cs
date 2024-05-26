using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachments : MonoBehaviour
{
    public enum Gun { M4, GLOCK };
    public Gun gun;
    public virtual void AttachBarrel(GameObject attachment, GameObject button) { }
    public virtual void AttachSight(GameObject attachment, GameObject button) { }

    public virtual void AttachUnderBarrel(GameObject attachment, GameObject button) { }

    public virtual void UnequipSight() { }

    public virtual void UnequipBarrel() { }

    public virtual void UnequipUnderBarrel() { }

}
