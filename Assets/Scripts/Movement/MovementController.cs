using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
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
    private Rigidbody rb;
    private bool isGrounded;

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
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }
    void HandleMovement()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        bool isSprinting = sprintAction.ReadValue<float>() > 0 && moveInput.y > 0;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 move = transform.forward * moveInput.y;
        transform.position += move * currentSpeed * Time.deltaTime;

        animator.SetFloat("Speed", moveInput.y * (isSprinting ? 2f : 1f));
    }

    void HandleMouseLook()
    {
        lookInput = lookAction.ReadValue<Vector2>();

        xRotation -= lookInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += lookInput.x * mouseSensitivity;;
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

     

        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
    void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
 
    void OnJump()
    {

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }

}
