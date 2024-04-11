using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float horizontal;
    float vertical;
    public float speed;
    CharacterController ch;
    Vector3 move;
    public Transform groundCheckPos;

    public float slopeRayRange;
    public float downForce;

    public float gravityVal = -9.81f;
    public float gravityMultiplier;
    public float gravity;

    // Start is called before the first frame update
    void Start()
    {
        ch = GetComponent<CharacterController>();
    }


    bool GroundCheck()
    {
        if(Physics.Raycast(groundCheckPos.position, -Vector3.up, out RaycastHit hit, 0.2f))
        {
            return true;
        }
        return false;
    }

    bool OnSlope()
    {
        if (Physics.Raycast(groundCheckPos.position, Vector3.down, out RaycastHit hit, slopeRayRange))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;




    }

    // Update is called once per frame
    void Update()
    {

        Gravity();

        if (OnSlope())
        {
            ch.Move(Vector3.down * downForce * Time.deltaTime);


        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        move = new Vector3(horizontal, gravity, vertical);

        Vector3 moveDir = (transform.forward * move.z + transform.right * move.x).normalized;

        Vector3 moveVec = new Vector3(moveDir.x, gravity, moveDir.z);

        ch.Move(moveVec * speed * Time.deltaTime);
    }

    void Gravity()
    {
        gravity = Mathf.Clamp(gravity, -9.81f, 0);
        if (!GroundCheck())
        {
            gravity -= Time.deltaTime * gravityMultiplier;
        }
        else
        {
            gravity = 0;
        }
    }
}
