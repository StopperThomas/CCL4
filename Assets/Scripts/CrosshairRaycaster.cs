using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairRaycaster : MonoBehaviour
{
    public float rayDistance = 5f;
    public InteractionManager interactionManager;
    public InspectUIManager uiManager;

    private GameObject lastHighlighted;

    void Start()
    {
        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<InteractionManager>();
            if (interactionManager == null)
                Debug.LogError("⚠️ InteractionManager not found!");
        }

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<InspectUIManager>();
            if (uiManager == null)
                Debug.LogWarning("⚠️ InspectUIManager not assigned!");
        }
    }
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += ctx => InteractWithObject();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    void Update()
    {
        if (interactionManager == null) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObj = hit.collider.gameObject;
            currentTarget = hitObj;

            // Highlight logic
            Screw screw = hitObj.GetComponent<Screw>();
            if (screw != null)
            {
                var equipped = interactionManager.GetEquippedItem();
                if (equipped != null && equipped.itemType == Item.ItemType.ScrewDriver)
                {
                    Highlight(hitObj);
                    uiManager?.ShowPrompt(true, "LMB"); // Show prompt for unscrewing
                }
                else
                {
                    RemoveHighlight();
                    uiManager?.ShowPrompt(false);
                }
                return;
            }

            // Inventory items or doors
            if (hitObj.CompareTag("InventoryItem"))
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "E");
                return;
            }

            if (hitObj.CompareTag("SceneChanger"))
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "E");
                return;
            }

            // Default fallback
            RemoveHighlight();
            uiManager?.ShowPrompt(false);
        }
        else
        {
            RemoveHighlight();
            uiManager?.ShowPrompt(false);
        }
    }

    void InteractWithObject()
    {
        if (currentTarget == null) return;

        // Handle Box interaction
        Box box = currentTarget.GetComponent<Box>();
        if (box != null)
        {
            box.TryOpen();
            return;
        }

        // Add other interaction logic here as needed (e.g., screws, scene changers)
    }

    void ApplyHighlight(GameObject obj)
    {
        if (lastHighlighted != null && lastHighlighted != obj)
        {
            Outline old = lastHighlighted.GetComponent<Outline>();
            if (old != null) old.enabled = false;
        }

        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
            lastHighlighted = obj;
        }
    }

    void RemoveHighlight()
    {
        if (lastHighlighted != null)
        {
            Outline outline = lastHighlighted.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
            lastHighlighted = null;
        }
    }
}
