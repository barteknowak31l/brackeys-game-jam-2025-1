using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
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
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentTilt = 0f;
    private float targetTiltDirection = 1f;
    private Rigidbody rb;
    private bool isGrounded;
    private float inputY = 0;

    private bool isImpact = false;
    private bool canTilt = false;
    private bool hasFallen = false;
    private bool instantKill = false;
    public RectTransform tiltArrow;
    public RectTransform tiltBar;
    public float maxArrowOffset = 50f;
    private Coroutine tiltCoroutine;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        tiltCoroutine = StartCoroutine(ChangeTiltDirection());
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ImpactTilt(2.5f, -1f)); 
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InstantKill(); 
        }
        HandleMovement();
        HandleMouseLook();
        HandleTilting();
        UpdateTiltUI();

    }

    void HandleMovement()
    {
        if (hasFallen) return;

        moveInput = moveAction.ReadValue<Vector2>();
         isSprinting = sprintAction.ReadValue<float>() > 0 && moveInput.y > 0;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 move = transform.forward * moveInput.y;
        transform.position += move * currentSpeed * Time.deltaTime;

        if (moveInput.y > 0)
        {
            inputY = 1;
        }
        else if ((moveInput.y < 0))
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
        if (!canTilt) return;
        if (Input.GetKey(KeyCode.A) && !instantKill)
        {
            currentTilt += tiltSpeed * Time.deltaTime * 50f;
        }
        if (Input.GetKey(KeyCode.D) && !instantKill)
        {
            currentTilt -= tiltSpeed * Time.deltaTime * 50f;
        }

        if (isSprinting)
        {
            currentTilt += targetTiltDirection * tiltSpeed * tiltMultiplier * 1.5f * Time.deltaTime * 10f;

        }
        else
        {
            currentTilt += targetTiltDirection * tiltSpeed * tiltMultiplier * Time.deltaTime * 10f;

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
            FallOver();
        }
    }

    void FallOver()
    {
        playerInput.enabled = false;
        hasFallen = true;

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
            targetTiltDirection = Random.Range(0, 2) * 2 - 1; 
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
        if (isGrounded)
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
        instantKill = true;
        tiltSpeed *= 15f; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Spawn"))
        {
            canTilt = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Spawn"))
        {
            canTilt = false;

            StartCoroutine(ResetTiltGradually());
        }
    }
    IEnumerator ResetTiltGradually()
    {
        while (Mathf.Abs(currentTilt) > 0.1f) 
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * 2f); 
            Quaternion tiltRotation = Quaternion.Euler(0f, 0f, currentTilt);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f) * tiltRotation;

            yield return null; 
        }

        currentTilt = 0f; 
        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

}
