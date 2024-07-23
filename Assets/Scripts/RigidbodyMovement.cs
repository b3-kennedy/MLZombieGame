using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RigidbodyMovement : MonoBehaviour
{

    public enum PlayerState {NORMAL, CROUCH, SPRINT, BANDAGING};
    public PlayerState playerState;

    public static RigidbodyMovement Instance;

    public float normalSpeed;
    public float crouchSpeed;
    public float sprintSpeed;

    public GameObject mlIdentifier;

    float currentSpeed;

    public Transform orientation;
    public Transform groundCheckPos;

    float horizontal;
    float vertical;
    public float slowDownMultiplier;

    Vector3 moveDir;

    [HideInInspector] public Rigidbody rb;

    public float groundDrag;

    [HideInInspector] public bool isCrouched;

    float timer;

    public float jumpForce;
    public float airDrag;

    public float audioRange = 5f;
    public LayerMask zombieLayer;

    [Header("Sprint Settings")]
    public AudioSource sprintAudioSource;
    public float maxSprintTime = 5f;
    public float sprintRecoveryTime;
    public float sprintTimer;
    public float recoveryTimer;
    [HideInInspector] public bool canSprint = true;
    bool isSprinting;


    [Header("Lean Settings")]
    public float leanAmount;
    public Transform hip;
    public Transform camRot;

    [Header("Crouch Settings")]
    public Transform crouchCamPos;
    public Transform normalCamPos;
    public Transform cameraHolder;
    public Transform crouchCastPoint;
    public float crouchLerpSpeed;
    public Collider crouchCol;
    Collider normalCol;
    bool objectAbove;

    [Header("Audio Settings")]
    public float walkFootstepInterval;
    public float sprintFootstepInterval;
    public float crouchFootstepInterval;
    public float crouchVolume;
    public float normalVolume;

    float defaultNormalVol;
    float defaultCrouchVol;

    float footstepTimer;
    PlayerAudioManager pam;


    float mlIdentifierTimer;

    private void Awake()
    {
        pam = GetComponent<PlayerAudioManager>();
        Instance = this;
        rb = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(7, 12);
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
        cameraHolder.GetChild(0).GetChild(0).GetChild(0).GetComponent<AmmoHolder>().player = gameObject;

        if (SettingsMenuManager.Instance != null)
        {
            SettingsMenuManager.Instance.updatedPlayerFoostepsVolume.AddListener(OnFootstepVolumeChange);
        }
        defaultNormalVol = normalVolume;
        defaultCrouchVol = crouchVolume;
    }

    public void OnFootstepVolumeChange()
    {
        if(SettingsMenuManager.Instance.playerFoostepsVolumeValue == 0)
        {
            normalVolume = 0;
            crouchVolume = 0;
        }
        else
        {
            normalVolume = defaultNormalVol * (SettingsMenuManager.Instance.playerFoostepsVolumeValue / 100f);
            crouchVolume = defaultCrouchVol * (SettingsMenuManager.Instance.playerFoostepsVolumeValue / 100f);
        }

    }

    void CheckAudioRange(Vector3 footStepPos)
    {
        if (!isCrouched)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, audioRange, zombieLayer);

            foreach (var col in cols)
            {
                if (col.GetComponent<ZombiePatrolAI>())
                {
                    col.GetComponent<ZombiePatrolAI>().HeardSound(footStepPos);
                }
                else if (col.GetComponent<EnforcerZombieAI>())
                {
                    col.GetComponent<EnforcerZombieAI>().HeardSound(footStepPos);
                }

            }
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");



        Vector2 moveVec = new Vector2(horizontal, vertical);


        
        SpeedControl();
        Crouch();
        Jump();
        Lean();
        CheckAudioRange(transform.position);

        if (Input.GetKey(KeyCode.LeftShift) && !objectAbove && canSprint)
        {
            playerState = PlayerState.SPRINT;
            ForceUncrouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !objectAbove)
        {
            isSprinting = false;
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

        if (mlIdentifier.activeSelf)
        {
            mlIdentifierTimer += Time.deltaTime;
            if (mlIdentifierTimer >= 5f)
            {
                if (mlIdentifier != null)
                {
                    mlIdentifier.SetActive(false);
                }

                mlIdentifierTimer = 0;
            }
        }

        if (!canSprint)
        {
            recoveryTimer += Time.deltaTime;
            if (recoveryTimer >= sprintRecoveryTime)
            {
                recoveryTimer = 0;
                canSprint = true;
            }
        }

        if(!isSprinting && sprintTimer > 0)
        {
            if(sprintTimer > 0)
            {
                sprintTimer -= Time.deltaTime;
            }
            
        }

    }

    void Lean()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            hip.transform.localRotation = Quaternion.Slerp(hip.transform.localRotation, Quaternion.Euler(0, 0, leanAmount), Time.deltaTime * 5);
            camRot.transform.localRotation = Quaternion.Slerp(camRot.transform.localRotation, Quaternion.Euler(0, 0, leanAmount), Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            hip.transform.localRotation = Quaternion.Slerp(hip.transform.localRotation, Quaternion.Euler(0, 0, -leanAmount), Time.deltaTime * 5);
            camRot.transform.localRotation = Quaternion.Slerp(camRot.transform.localRotation, Quaternion.Euler(0, 0, -leanAmount), Time.deltaTime * 5);
        }
        else
        {
            hip.transform.localRotation = Quaternion.Slerp(hip.transform.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * 5);
            camRot.transform.localRotation = Quaternion.Slerp(camRot.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 5);
        }
    }


    void ForceUncrouch()
    {
        if (isCrouched && !objectAbove)
        {
            isCrouched = false;
            Debug.Log("uncrouch");
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            ForceUncrouch();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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



    void FootstepAudio(float intervalMax)
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (inputVec != Vector3.zero)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= intervalMax)
            {
                FootStepSelect();
                footstepTimer = 0;
            }
        }
    }


    void FootStepSelect()
    {
        if (Physics.Raycast(groundCheckPos.position, -Vector3.up, out RaycastHit hit, 0.2f))
        {
            if (hit.collider.GetComponent<Material>())
            {
                Material mat = hit.collider.GetComponent<Material>();
                switch (mat.matType)
                {
                    case Material.MaterialType.GRASS:
                        pam.PlayFootstep(pam.grassStepsWalk);
                        break;
                    case Material.MaterialType.CONCRETE:
                        pam.PlayFootstep(pam.concreteStepsWalk);
                        break;
                    default:
                        pam.PlayFootstep(pam.grassStepsWalk);
                        break;
                }
            }
        }
    }

    private void Move()
    {
        moveDir = orientation.forward * vertical + orientation.right * horizontal;
        if (IsGrounded())
        {
            switch (playerState)
            {
                case (PlayerState.NORMAL):
                    rb.AddForce(moveDir * normalSpeed * 10, ForceMode.Force);
                    pam.footstepSource.volume = normalVolume;
                    FootstepAudio(walkFootstepInterval);
                    currentSpeed = normalSpeed;

                    break;
                case (PlayerState.CROUCH):
                    rb.AddForce(moveDir * crouchSpeed * 10, ForceMode.Force);
                    pam.footstepSource.volume = crouchVolume;
                    FootstepAudio(crouchFootstepInterval);
                    currentSpeed = crouchSpeed;
                    break;
                case (PlayerState.SPRINT):
                    UnAim();
                    Sprint();
                    rb.AddForce(moveDir * sprintSpeed * 10, ForceMode.Force);
                    pam.footstepSource.volume = normalVolume;
                    FootstepAudio(sprintFootstepInterval);
                    currentSpeed = sprintSpeed;
                    break;
                case (PlayerState.BANDAGING):
                    rb.AddForce(moveDir * crouchSpeed * 10, ForceMode.Force);
                    pam.footstepSource.volume = crouchVolume;
                    FootstepAudio(crouchFootstepInterval);
                    currentSpeed = crouchSpeed;
                    break;


            }
        }
        else
        {
            rb.AddForce(moveDir * normalSpeed * 10 * airDrag, ForceMode.Force);
        }


       
    }

    void Sprint()
    {
        
        if(rb.velocity != Vector3.zero)
        {
            isSprinting = true;
            sprintTimer += Time.deltaTime;
        }
        
        if(sprintTimer >= maxSprintTime)
        {
            playerState = PlayerState.NORMAL;
            isSprinting = false;
            canSprint = false;
            sprintTimer = 0;
            sprintAudioSource.Play();
        }


    }

    void UnAim()
    {
        ADS adsScript;
        if (GetComponent<PickUpWeapons>().weaponPos.childCount > 0)
        {
            adsScript = GetComponent<PickUpWeapons>().currentActiveGun.transform.GetChild(0).GetComponent<ADS>();
            adsScript.isAiming = false;
            adsScript.aimedOut.Invoke();
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

