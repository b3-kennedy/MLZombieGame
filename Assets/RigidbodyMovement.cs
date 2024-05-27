using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    public static RigidbodyMovement Instance;

    public float speed;

    public Transform orientation;
    public Transform groundCheckPos;

    float horizontal;
    float vertical;
    public float slowDownMultiplier;

    Vector3 moveDir;

    [HideInInspector] public Rigidbody rb;

    public float groundDrag;

    float timer;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
    }

    public bool IsGrounded()
    {
        if (Physics.Raycast(groundCheckPos.position, -Vector3.up, out RaycastHit hit, 0.2f))
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        Vector2 moveVec = new Vector2(horizontal, vertical);


        Move();
        SpeedControl();

        if (IsGrounded())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void Move()
    {
        moveDir = orientation.forward * vertical + orientation.right * horizontal;
        rb.AddForce(moveDir * speed * 10, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(vel.magnitude > speed)
        {
            Vector3 limitedVel = vel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}

