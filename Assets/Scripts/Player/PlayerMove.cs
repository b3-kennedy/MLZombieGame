using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{


    public enum PlayerState {NORMAL, CROUCH, SPRINT};
    public PlayerState playerState = PlayerState.NORMAL;

    float horizontal;
    float vertical;
    public float normalSpeed;
    public float crouchSpeed;
    public float sprintSpeed;
    [HideInInspector] public CharacterController ch;
    Vector3 move;
    Vector3 moveVec;
    public Transform groundCheckPos;

    public float slopeRayRange;
    public float downForce;

    public float gravityVal = -9.81f;
    public float gravityMultiplier;
    public float gravity;
    Vector3 velocity;

    bool sprint;

    public Transform hip;
    Vector3 defaultCamPos;
    Quaternion defaultCamRot;

    [Header("Crouch Parameters")]
    public Transform crouchCastPoint;
    bool crouch;
    public bool objectAbove;
    public GameObject cam;
    public Vector3 crouchCamPos;
    Vector3 normalCamPos;
    public float crouchLerpSpeed;

    [Header("Jump Parameters")]
    public float jumpHeight;

    [HideInInspector] public float currentSpeed;

    [Header("Lean Parameters")]
    public float leanDistance;
    public float leanSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ch = GetComponent<CharacterController>();
        normalCamPos = cam.transform.localPosition;
        defaultCamPos = cam.transform.localPosition;
        defaultCamRot = cam.transform.localRotation;
    }


    public bool GroundCheck()
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
        Move();
        Slope();
        Crouch();
        Jump();
        



        

    }


    void Lean()
    {

    }

    void Slope()
    {
        if (OnSlope())
        {
            ch.Move(Vector3.down * downForce * Time.deltaTime);


        }
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        move = new Vector3(horizontal, gravity, vertical);

        Vector3 moveDir = (transform.forward * move.z + transform.right * move.x).normalized;

        moveVec = new Vector3(moveDir.x, gravity, moveDir.z);


        if (Input.GetKey(KeyCode.LeftShift) && !objectAbove)
        {
            playerState = PlayerState.SPRINT;
            sprint = true;
            ForceUncrouch();

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !objectAbove)
        {
            playerState = PlayerState.NORMAL;
            sprint = false;
        }

        switch (playerState)
        {
            case PlayerState.NORMAL:
                ch.Move(moveDir * normalSpeed * Time.deltaTime);
                currentSpeed = normalSpeed;
                break;
            case PlayerState.CROUCH:
                ch.Move(moveDir * crouchSpeed * Time.deltaTime);
                currentSpeed = crouchSpeed;
                break;
            case PlayerState.SPRINT:
                ch.Move(moveDir * sprintSpeed * Time.deltaTime);
                currentSpeed = sprintSpeed;
                break;

        }
        
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

        if (Input.GetKeyDown(KeyCode.Space) && (GroundCheck() || OnSlope()))
        {
            velocity.y = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityVal);

        }

        velocity.y += gravity;
        ch.Move(velocity * Time.deltaTime);
    }

    void ForceUncrouch()
    {
        if(crouch && !objectAbove)
        {
            playerState = PlayerState.NORMAL;
            crouch = false;
            ch.height = 2;
            ch.center = new Vector3(0, 0, 0);
        }
    }
    void Crouch()
    {

        if (!crouch && Input.GetKeyDown(KeyCode.C))
        {
            crouch = true;
            playerState = PlayerState.CROUCH;
            ch.height = 1;
            ch.center = new Vector3(0, -0.5f, 0);
            
        }
        else if(crouch && Input.GetKeyDown(KeyCode.C) && !objectAbove)
        {
            playerState = PlayerState.NORMAL;
            crouch = false;
            ch.height = 2;
            ch.center = new Vector3(0, 0, 0);
        }


        if (crouch)
        {

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, crouchCamPos, Time.deltaTime * crouchLerpSpeed);

            //Debug.DrawRay(crouchCastPoint.position, transform.up * 1);
            if(Physics.Raycast(crouchCastPoint.position, transform.up, out RaycastHit hit, 1f))
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
        }
        else
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, normalCamPos, Time.deltaTime * crouchLerpSpeed);
        }
    }

    void Jump()
    {

    }


}
