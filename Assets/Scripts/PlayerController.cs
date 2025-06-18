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

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float cameraCrouchOffset = -0.5f;

    [Header("Inventory Settings")]
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inspectionUI;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private InteractionManager interactionManager;

    private Inventory inventory;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    private bool isInventoryOpen = false;

    private bool isCrouching = false;
    private float targetHeight;
    private Vector3 cameraInitialLocalPos;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
        interactionManager = FindObjectOfType<InteractionManager>();

        cameraInitialLocalPos = cameraTransform.localPosition;
        targetHeight = standHeight;

        StartCoroutine(WaitForInventoryManager());
    }

    private System.Collections.IEnumerator WaitForInventoryManager()
    {
        while (InventoryManager.Instance == null)
        {
            Debug.Log("⏳ Waiting for InventoryManager to be initialized...");
            yield return null;
        }

        inventory = InventoryManager.Instance.inventory;

        if (uiInventory != null)
            uiInventory.SetInventory(inventory);
    }

    private void OnEnable()
    {
        inputActions?.Player.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Crouch.performed += ctx => ToggleCrouch();

        inputActions.Player.Inventory.performed += ctx => ToggleInventory();
        inputActions.Player.DropItem.performed += ctx => TryDropSelectedItem();
        inputActions.Player.EquipItem.performed += ctx => TryEquipInspectedItem();
        inputActions.Player.UnequipItem.performed += ctx => interactionManager?.UnequipItem();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;

        inputActions.Player.Crouch.performed -= ctx => ToggleCrouch();

        inputActions.Player.Inventory.performed -= ctx => ToggleInventory();
        inputActions.Player.DropItem.performed -= ctx => TryDropSelectedItem();
        inputActions.Player.EquipItem.performed -= ctx => TryEquipInspectedItem();
        inputActions.Player.UnequipItem.performed -= ctx => interactionManager?.UnequipItem();
        inputActions.Player.Disable();
    }

    private void Update() 
    {
        if (isInventoryOpen) return;

        HandleGroundCheck();
        HandleGravity();
        HandleMovement();
        HandleLook();
        HandleCrouch();
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

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        targetHeight = isCrouching ? crouchHeight : standHeight;
    }

    private void HandleCrouch()
    {
        // Smoothly transition character controller height
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchSpeed);

        // Adjust camera to simulate crouching
        float targetY = cameraInitialLocalPos.y + (isCrouching ? cameraCrouchOffset : 0f);
        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetY, Time.deltaTime * crouchSpeed);
        cameraTransform.localPosition = camPos;
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

    private void TryDropSelectedItem()
    {
        if (!isInventoryOpen) return;

        Item selectedItem = uiInventory.GetSelectedItem();
        if (selectedItem == null) return;

        var inspector = FindObjectOfType<ItemInspectorUI>();
        if (inspector != null)
        {
            inspector.ShowItem(selectedItem);
            inspector.TryDropItemInput();
        }
    }

    private void TryEquipInspectedItem()
    {
        if (!isInventoryOpen) return;

        var inspector = FindObjectOfType<ItemInspectorUI>();
        if (inspector != null)
        {
            inspector.TryEquipInspectedItem();
        }
    }
}