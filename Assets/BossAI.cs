using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    public Transform target;

    ThrowAttack throwAttack;

    // Start is called before the first frame update
    void Start()
    {
        throwAttack = GetComponent<ThrowAttack>();
        throwAttack.Execute();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPoint(target.position);
    }

    void LookAtPoint(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Quaternion toRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), new Quaternion(0, toRot.y, 0, toRot.w), 2 * Time.deltaTime);
    }
}
