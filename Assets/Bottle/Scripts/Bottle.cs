using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    public AudioClip glassSmash;
    bool exploded;
    
    void Update() // just for testing
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled && collision.gameObject.layer != 7 && !exploded)
        {
            
            if (GetComponent<AudioEmitter>())
            {
                GetComponent<AudioEmitter>().Alert();
            }
            Explode();
        }

    }

    public void Explode()
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.GetChild(0).position, Quaternion.identity);
        
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
        exploded = true;
        AudioSource.PlayClipAtPoint(glassSmash, transform.position);
    }
}
