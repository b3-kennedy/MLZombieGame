using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{

    public enum PlayerState {NORMAL, CROUCH, SPRINT };
    public PlayerState playerState;

    public static RigidbodyMovement Instance;

    public float normalSpeed;
    public float crouchSpeed;
    public float sprintSpeed;

    float currentSpeed;

    public Transform orientation;
    public Transform groundCheckPos;

    float horizontal;
    float vertical;
    public float slowDownMultiplier;

    Vector3 moveDir;

    [HideInInspector] public Rigidbody rb;

    public float groundDrag;

    bool isCrouched;

    float timer;

    [Header("Crouch Settings")]
    public Transform crouchCamPos;
    public Transform normalCamPos;
    public Transform cameraHolder;
    public Transform crouchCastPoint;
    public float crouchLerpSpeed;
    public Collider crouchCol;
    Collider normalCol;
    bool objectAbove;

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
        normalCol = GetComponent<Collider>();
        playerState = PlayerState.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


        Vector2 moveVec = new Vector2(horizontal, vertical);


        Move();
        SpeedControl();
        Crouch();

        if (Input.GetKey(KeyCode.LeftShift) && !objectAbove)
        {
            playerState = PlayerState.SPRINT;
            ForceUncrouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !objectAbove)
        {
            playerState = PlayerState.NORMAL;
        }

        if (IsGrounded())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }


    void ForceUncrouch()
    {
        if (!isCrouched && !objectAbove)
        {
            isCrouched = false;
            Debug.Log("uncrouch");
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isCrouched)
        {
            isCrouched = true;
            playerState = PlayerState.CROUCH;
        }
        else if(Input.GetKeyDown(KeyCode.C) && isCrouched && !objectAbove)
        {
            playerState = PlayerState.NORMAL;
            isCrouched = false;
        }

        if (isCrouched)
        {

            if (Physics.Raycast(crouchCastPoint.position, transform.up, out RaycastHit hit, 1f))
            {
                if (hit.collider)
                {
                    objectAbove = true;
                    Debug.Log("cant uncrouch");
                }
            }
            else
            {
                objectAbove = false;
            }

            crouchCol.enabled = true;
            normalCol.enabled = false;
            cameraHolder.position = Vector3.Lerp(cameraHolder.position, crouchCamPos.position, Time.deltaTime * crouchLerpSpeed);
        }
        else if(!isCrouched)
        {
            normalCol.enabled = true;
            crouchCol.enabled = false;
            cameraHolder.position = Vector3.Lerp(cameraHolder.position, normalCamPos.position, Time.deltaTime * crouchLerpSpeed);
        }

    }

    private void Move()
    {
        moveDir = orientation.forward * vertical + orientation.right * horizontal;
        switch (playerState)
        {
            case (PlayerState.NORMAL):
                rb.AddForce(moveDir * normalSpeed * 10, ForceMode.Force);
                currentSpeed = normalSpeed;
                break;
            case (PlayerState.CROUCH):
                rb.AddForce(moveDir * crouchSpeed * 10, ForceMode.Force);
                currentSpeed = crouchSpeed;
                break;
            case (PlayerState.SPRINT):
                rb.AddForce(moveDir * sprintSpeed * 10, ForceMode.Force);
                currentSpeed = sprintSpeed;
                break;

        }
       
    }

    void SpeedControl()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(vel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = vel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}

