using System;
using AudioManager;
using Observers.dto;
using StateMachine;
using StateMachine.states;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MovementController : MonoBehaviour, Observers.IObserver<WindDTO>, Observers.IObserver<AnvilDTO>, Observers.IObserver<StormDTO>, Observers.IObserver<StateDTO>, Observers.IObserver<UfoDTO>, Observers.IObserver<FruitDTO>, Observers.IObserver<BeaverDTO>, Observers.IObserver<SharkDTO>, Observers.IObserver<BirdDTO>
{
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public float tiltSpeed = 1f;
    public float maxTiltAngle = 20f;
    public float tiltMultiplier = 1f;
    private bool isSprinting = false;

    public Transform cameraTransform;
    public Animator animator;
    public LayerMask groundLayer;

    private PlayerInput playerInput;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction kickAction;
    private InputAction moveForwardAction;
    private InputAction moveBackwardAction;
    private InputAction tiltLeftAction;
    private InputAction tiltRightAction;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentTilt = 0f;
    private float targetTiltDirection = 1f;
    private Rigidbody rb;
    private bool isGrounded;
    private float inputY = 0;

    private bool isWindActive = false;
    private bool isImpact = false;
    private bool canTilt = true;
    private bool hasFallen = false;
    private bool isFalling = false;
    private bool instantKill = false;
    public RectTransform tiltArrow;
    public RectTransform tiltBar;
    public float maxArrowOffset = 50f;
    private Coroutine tiltCoroutine;
    public ParticleSystem windLeft;
    public ParticleSystem windRight;
    public ParticleSystem rain;
    public WindState windState;
    public AnvilState anvilState;
    public StormState stormState;
    public UfoState ufoState;
    public FruitState fruitState;
    public BeaverState beaverState;
    public SharkState sharkState;
    public BirdState birdState;
    private bool isCrouching;
    private float randomTiltChange = 0f;
    private float tiltSpeedMultiplier = 1f;
    public SmoothCameraSwitch cam;

    private Coroutine resetTiltCoroutine;
    private CapsuleCollider capsuleCollider;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;
    private Vector3 originalCameraPosition;
    public GameObject body;
    public GameObject body2;

    private Coroutine ufoLiftCoroutine;
    private bool isBeingLifted = false;
    private Vector3 liftStartPosition;
    private bool wasSprinting;


    public float randomTiltChangeMin = 0.8f;
    public float randomTiltChangeMax = 1.8f;
    public float sprintTiltMultiplier = 2f;
    public AudioSource audioSource;
    [Header("Ufo stuff")]
    [SerializeField] private float _ufoLiftSpeed = 3.0f;
    [SerializeField] private float _ufoLiftThreshold = 5.0f;

    [Header("Bird stuff")]
    [SerializeField] private ParticleSystem _explosionParticleSystem;
    [SerializeField] private AudioSource _explosionAudioSource;
    
    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalColliderHeight = capsuleCollider.height;
        originalCameraPosition = cameraTransform.localPosition;

        playerInput = GetComponent<PlayerInput>();
        moveForwardAction = playerInput.actions["MoveForward"];
        moveBackwardAction = playerInput.actions["MoveBackward"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        crouchAction = playerInput.actions["Crouch"];
        kickAction = playerInput.actions["Kick"];
        tiltLeftAction = playerInput.actions["tiltLeft"];
        tiltRightAction = playerInput.actions["tiltRight"];
        kickAction = playerInput.actions["Kick"];
        kickAction.performed += _ => Kick();
        crouchAction.started += _ => StartCrouch();
        crouchAction.canceled += _ => StopCrouch();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalColliderCenter = capsuleCollider.center;

        tiltCoroutine = StartCoroutine(ChangeTiltDirection());
    }

    private void Start()
    {
        _explosionAudioSource = gameObject.AddComponent<AudioSource>();
        _explosionAudioSource.playOnAwake = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //    StartCoroutine(ImpactTilt(2.5f, -1f)); 
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //     InstantKill(); 
        }

        if (SmoothCameraSwitch.currentCameraIndex == 0)
        {

        }
        else
        {
            HandleMovement();
            HandleMouseLook();
            HandleTilting();
            UpdateTiltUI();
        }

    }

    void OnEnable()
    {
        isFalling = false;
        windState.AddObserver(this);
        anvilState.AddObserver(this);
        stormState.AddObserver(this);
        ufoState.AddObserver(this);
        fruitState.AddObserver(this);
        beaverState.AddObserver(this);
        sharkState.AddObserver(this);
        birdState.AddObserver(this);
        StateMachineManager.instance.AddObserver(this);
    }

    void OnDisable()
    {
        windState.RemoveObserver(this);
        anvilState.RemoveObserver(this);
        stormState.RemoveObserver(this);
        ufoState.RemoveObserver(this);
        fruitState.RemoveObserver(this);
        beaverState.RemoveObserver(this);
        sharkState.RemoveObserver(this);
        birdState.RemoveObserver(this);
        StateMachineManager.instance.RemoveObserver(this);
    }


    void ResetPlayer()
    {
        audioSource.enabled = true;

        isFalling = false;
        hasFallen = false;
        instantKill = false;
        currentTilt = 0f;
        playerInput.enabled = true;
        tiltCoroutine = StartCoroutine(ChangeTiltDirection());
        tiltMultiplier = 1f;
        tiltSpeed = 0.5f;
        rain.Stop();
        rain.Clear();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    void StartCrouch()
    {
        if (!isGrounded) return;

        isCrouching = true;
        animator.SetBool("Crouch", true);
        capsuleCollider.height = originalColliderHeight / 2f;
        capsuleCollider.center = new Vector3(originalColliderCenter.x, originalColliderCenter.y - originalColliderHeight / 4f, originalColliderCenter.z);

        StartCoroutine(CrouchCameraTransition(originalCameraPosition - new Vector3(0, originalColliderHeight / 3.1f, 0), 0.2f));
    }

    void StopCrouch()
    {
        if (!isCrouching) return;

        isCrouching = false;
        animator.SetBool("Crouch", false);
        capsuleCollider.height = originalColliderHeight;
        capsuleCollider.center = originalColliderCenter;

        StartCoroutine(CrouchCameraTransition(originalCameraPosition, 0.2f));
    }
    void Kick()
    {
        if (animator != null && isGrounded && !isCrouching)
        {
            animator.SetBool("IsKicking", true);
            StartCoroutine(ResetKick());

        }
    }

    private IEnumerator ResetKick()
    {
        playerInput.actions["Jump"].Disable();
        playerInput.actions["Crouch"].Disable();
        playerInput.actions["Move"].Disable();
        playerInput.actions["MoveForward"].Disable();
        playerInput.actions["MoveBackward"].Disable();
        playerInput.actions["Kick"].Disable();
        body.SetActive(false);
        body2.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        body.SetActive(true);
        body2.SetActive(true);

        playerInput.actions["Jump"].Enable();
        playerInput.actions["Crouch"].Enable();
        playerInput.actions["Move"].Enable();
        playerInput.actions["MoveForward"].Enable();
        playerInput.actions["MoveBackward"].Enable();
        playerInput.actions["Kick"].Enable();
        animator.SetBool("IsKicking", false);

    }
    private IEnumerator CrouchCameraTransition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.localPosition;

        while (elapsedTime < duration)
        {
            cameraTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = targetPosition;
    }

    void HandleMovement()
    {
        if (hasFallen) return;

        float moveForward = moveForwardAction.ReadValue<float>();
        float moveBackward = moveBackwardAction.ReadValue<float>() * -1; // -1 to move backward; 
        float moveInput = moveForward + moveBackward;

        isSprinting = sprintAction.ReadValue<float>() > 0 && moveInput > 0 && !isCrouching;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 move = transform.forward * moveInput;
        transform.position += move * currentSpeed * Time.deltaTime;

        if (moveInput > 0)
        {
            inputY = 1;
        }
        else if ((moveInput < 0))
        {
            inputY = -1;
        }
        else
        {
            inputY = 0;
        }
        animator.SetFloat("Speed", inputY * (isSprinting ? 2f : 1f));
    }

    void HandleMouseLook()
    {
        lookInput = lookAction.ReadValue<Vector2>();

        xRotation -= lookInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += lookInput.x * mouseSensitivity;
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void HandleTilting()
    {
        Audio();
        if (!canTilt) return;

        randomTiltChange -= Time.deltaTime;
        if (randomTiltChange <= 0f)
        {
            tiltSpeedMultiplier = Random.Range(randomTiltChangeMin, randomTiltChangeMax);
            //  Debug.Log(tiltSpeedMultiplier);
            randomTiltChange = Random.Range(3f, 6f);
        }

        if (tiltLeftAction.ReadValue<float>()  == 1 && !instantKill)
        {
            currentTilt += tiltSpeed * Time.deltaTime * 50f;
        }
        if (tiltRightAction.ReadValue<float>() == 1 && !instantKill)
        {
            currentTilt -= tiltSpeed * Time.deltaTime * 50f;
        }


        if (isSprinting)
        {

            currentTilt += targetTiltDirection * tiltSpeed * tiltMultiplier * tiltSpeedMultiplier * sprintTiltMultiplier * Time.deltaTime * 10f;
        }
        else
        {

            currentTilt += targetTiltDirection * tiltSpeed * tiltMultiplier * tiltSpeedMultiplier * Time.deltaTime * 10f;
        }

        currentTilt = Mathf.Clamp(currentTilt, -maxTiltAngle, maxTiltAngle);

        Quaternion tiltRotation = Quaternion.Euler(0f, 0f, currentTilt);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f) * tiltRotation;

        if (Mathf.Abs(currentTilt) >= maxTiltAngle)
        {
            instantKill = true;
            hasFallen = true;
        }
        if (hasFallen)
        {
            audioSource.enabled = false;

            FallOver();
        }
    }
    private void Audio()
    {
        bool isMoving = moveForwardAction.ReadValue<float>() > 0 || moveBackwardAction.ReadValue<float>() > 0;

        if (isMoving && audioSource.enabled)
        {

            if (isSprinting)
            {
                if (!audioSource.isPlaying || wasSprinting == false)
                {
                    wasSprinting = true;
                    audioSource.loop = true;
                    AudioManager.AudioManager.PlaySound(AudioClips.SprintSteps, audioSource, 1);
                }
            }
            else
            {
                if (!audioSource.isPlaying || wasSprinting == true)
                {
                    wasSprinting = false;
                    audioSource.loop = true;
                    AudioManager.AudioManager.PlaySound(AudioClips.WalkingSteps, audioSource, 1);
                }
            }
        }
        else
        {

            if (audioSource.isPlaying)
            {

                audioSource.Stop();
            }
        }
    }

    void FallOver()
    {
        playerInput.enabled = false;
        hasFallen = true;
        if (!isFalling)
        {
            StateMachineManager.instance.PlayerDeathState();
            isFalling = true;
        }

        if (tiltCoroutine != null)
        {
            StopCoroutine(tiltCoroutine);
        }

        float pushAmount = 0.008f;
        float rotateAmount = 0.1f;

        Vector3 pushDirection = transform.right * (currentTilt > 0 ? -pushAmount : pushAmount);
        Vector3 newRotation = transform.eulerAngles + new Vector3(0f, 0f, currentTilt > 0 ? rotateAmount : -rotateAmount);

        transform.position += pushDirection;
        transform.rotation = Quaternion.Euler(newRotation);

    }

    IEnumerator ChangeTiltDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            if (!isWindActive)
            {
                targetTiltDirection = Random.Range(0, 2) * 2 - 1;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = true;
            //     animator.SetBool("IsJumping", false);
        }
    }

    void OnJump()
    {
        if (isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //     animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }

    void UpdateTiltUI()
    {
        if (hasFallen) return;

        if (tiltArrow == null || tiltBar == null) return;

        float normalizedTilt = currentTilt / maxTiltAngle;
        float arrowX = -normalizedTilt * maxArrowOffset;

        tiltArrow.anchoredPosition = new Vector2(arrowX, tiltArrow.anchoredPosition.y);
    }
    IEnumerator ImpactTilt(float impactTiltSpeed, float direction)
    {
        if (isImpact) yield break;
        isImpact = true;

        float originalTiltSpeed = tiltSpeed;
        tiltSpeed *= impactTiltSpeed;
        targetTiltDirection = direction;

        yield return new WaitForSeconds(2f);

        tiltSpeed = originalTiltSpeed;
        isImpact = false;
    }
    void InstantKill()
    {
        audioSource.enabled = false;

        instantKill = true;
        tiltSpeed *= 15f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Spawn"))
        {
            if (resetTiltCoroutine != null)
            {
                StopCoroutine(resetTiltCoroutine);
                resetTiltCoroutine = null;
            }
            canTilt = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Spawn"))
        {
            if (resetTiltCoroutine != null)
            {
                StopCoroutine(resetTiltCoroutine);
            }
            resetTiltCoroutine = StartCoroutine(ResetTilt(1f));
        }
    }

    private IEnumerator ResetTilt(float duration)
    {
        float startTilt = currentTilt;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            currentTilt = Mathf.Lerp(startTilt, 0f, elapsedTime / duration);
            Quaternion tiltRotation = Quaternion.Euler(0f, 0f, currentTilt);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f) * tiltRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentTilt = 0f;
        canTilt = false;
    }


    public void OnNotify(WindDTO dto)
    {
        if (dto._enabled)
        {
            isWindActive = true;
            tiltMultiplier = 2f;
            targetTiltDirection = (dto._direction == 0) ? 1f : -1f;

            if (tiltCoroutine != null)
            {
                StopCoroutine(tiltCoroutine);
                tiltCoroutine = null;
            }

            if (dto._direction == 0)
            {
                windLeft.Play();
                windRight.Stop();
                windRight.Clear();
            }
            else
            {
                windRight.Play();
                windLeft.Stop();
                windLeft.Clear();
            }
        }
        else
        {
            isWindActive = false;
            windLeft.Stop();
            windLeft.Clear();
            windRight.Stop();
            windRight.Clear();
            tiltMultiplier = 1f;

            if (tiltCoroutine == null)
            {
                tiltCoroutine = StartCoroutine(ChangeTiltDirection());
            }
        }
    }

    public void OnNotify(AnvilDTO dto)
    {
        InstantKill();
    }

    public void OnNotify(StormDTO dto)
    {
        InstantKill();
    }

    public void OnNotify(StateDTO dto)
    {


        switch (dto._state)
        {
            case States.Storm:
                {
                    if (dto._variant == Variant.Second)
                    {
                        tiltMultiplier = 2f;
                        rain.Play();
                    }
                    else
                    {
                        tiltMultiplier = 1f;
                        rain.Stop();
                        rain.Clear();
                    }
                    break;
                }
            case States.StartState:
                {
                    ResetPlayer();
                    StopExplosion();

                    break;
                }
        
            default:
                {
                    tiltMultiplier = 1f;
                    rain.Stop();
                    rain.Clear();
                    break;

                }

        }

    }



    public void OnNotify(UfoDTO dto)
    {
        if (dto._cowHit)
        {
            InstantKill();
        }

        if (dto._playerInBeam)
        {
            if (!isBeingLifted)
            {
                isBeingLifted = true;
                rb.isKinematic = true;
                playerInput.actions["Jump"].Disable();
                playerInput.actions["Crouch"].Disable();
                ufoLiftCoroutine = StartCoroutine(LiftPlayer());
            }
            SwapTiltControls(true); 




        }
        else
        {
            if (isBeingLifted)
            {
                isBeingLifted = false;
                if (ufoLiftCoroutine != null)
                {
                    StopCoroutine(ufoLiftCoroutine);
                    ufoLiftCoroutine = null;
                }
                rb.isKinematic = false;
                playerInput.actions["Jump"].Enable();
                playerInput.actions["Crouch"].Enable();
            }
            SwapTiltControls(false);

        }
    }
    private void SwapTiltControls(bool invert)
    {
        if (invert)
        {
  

            tiltRightAction = playerInput.actions["tiltLeft"];
            tiltLeftAction = playerInput.actions["tiltRight"];
        }
        else
        {
            tiltRightAction = playerInput.actions["tiltRight"];
            tiltLeftAction = playerInput.actions["tiltLeft"];
        
        }
    }


    private IEnumerator LiftPlayer()
    {

        while (isBeingLifted)
        {
            transform.position += new Vector3(0, _ufoLiftSpeed * Time.deltaTime, 0);

            if (transform.position.y >= _ufoLiftThreshold)
            {
                InstantKill();
                yield break;
            }

            yield return null;
        }
    }

    public void OnNotify(FruitDTO dto)
    {

        InstantKill();


    }
    public void OnNotify(SharkDTO dto)
    {

        InstantKill();


    }

    public void OnNotify(BeaverDTO dto)
    {
        if (dto._playerInCollider)
        {
            moveForwardAction.Disable();
        }
        else
        {
            moveForwardAction.Enable();

        }

        if (dto._endTime)
        {
            InstantKill();
        }

    }

    public void OnNotify(BirdDTO dto)
    {
        if (dto._variant == Variant.Second)
        {
            StartExplosion();
        }

        if (dto._damage == 2)
        {
            InstantKill();
        }
        else
        {
            StartCoroutine(ImpactTilt(2, 3));
        }

    }
    public void OnWallEnter()
    {
        moveBackwardAction.Disable();
    }
    public void OnWallExit()
    {
        moveBackwardAction.Enable();
    }

    private void StartExplosion()
    {
        _explosionParticleSystem.Play();
        AudioManager.AudioManager.PlaySound(AudioClips.Explosion, _explosionAudioSource, 1.0f);
    }

    private void StopExplosion()
    {
        _explosionParticleSystem.Stop();
        AudioManager.AudioManager.StopSound(AudioClips.Explosion, _explosionAudioSource);

    }


}

