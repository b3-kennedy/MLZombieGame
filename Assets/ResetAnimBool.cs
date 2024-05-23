using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimBool : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Reset()
    {
        anim.SetBool("shoot", false);
    }
}
