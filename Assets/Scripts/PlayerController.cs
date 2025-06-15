using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Inventory Settings")]
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject inventoryUI; 
    [SerializeField] private GameObject inspectionUI;        

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Inventory inventory;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    private bool jumpPressed = false;
    private bool isInventoryOpen = false; 

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        controller = GetComponent<CharacterController>();

        inventory = InventoryManager.Instance.inventory;
        if (uiInventory != null)
            uiInventory.SetInventory(inventory);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
        inputActions.Player.Jump.canceled += ctx => jumpPressed = false;

        inputActions.Player.Inventory.performed += ctx => ToggleInventory(); // <-- NEW
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Jump.performed -= ctx => jumpPressed = true;
        inputActions.Player.Jump.canceled -= ctx => jumpPressed = false;

        inputActions.Player.Inventory.performed -= ctx => ToggleInventory(); // <-- NEW
    }

    private void Update()
    {
        if (isInventoryOpen) return; // Disable movement/look if inventory is open

        HandleGroundCheck();
        HandleJump();
        HandleGravity();
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleGroundCheck()
    {
        Vector3 spherePosition = transform.position + Vector3.down * (controller.height / 2 - controller.radius + 0.05f);
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

private void ToggleInventory()
{
    isInventoryOpen = !isInventoryOpen;

    inventoryUI.SetActive(isInventoryOpen);
    inspectionUI.SetActive(isInventoryOpen);

    Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
    Cursor.visible = isInventoryOpen;

    
    var interactionManager = FindObjectOfType<InteractionManager>();
    if (interactionManager != null)
        interactionManager.enabled = !isInventoryOpen;

    
    if (!isInventoryOpen)
    {
        FindObjectOfType<ItemInspectorUI>()?.HideItem();
    }
}

}
