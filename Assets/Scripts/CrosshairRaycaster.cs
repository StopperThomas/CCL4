using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairRaycaster : MonoBehaviour
{
    public float rayDistance = 5f;
    public InspectUIManager uiManager;

    private GameObject currentTarget;
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObj = hit.collider.gameObject;
            currentTarget = hitObj;

            bool isInteractable = hitObj.CompareTag("Interactable");
            bool isScrew = hitObj.GetComponent<Screw>() != null;
            bool isScrewdriver = hitObj.CompareTag("Screwdriver");
            bool isDoor = hitObj.CompareTag("SceneChanger");
            bool isBox = hitObj.GetComponent<Box>() != null;

            if (isInteractable || isScrew || isScrewdriver || isDoor || isBox)
            {
                ApplyHighlight(hitObj);

                // Show appropriate UI prompt
                if (isInteractable || isDoor || isBox)
                    uiManager.ShowPrompt(true, "E");
                else if (isScrew || isScrewdriver)
                    uiManager.ShowPrompt(true, "LMB");
            }
            else
            {
                ClearHighlight();
                uiManager.ShowPrompt(false);
            }
        }
        else
        {
            ClearHighlight();
            uiManager.ShowPrompt(false);
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
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void ClearHighlight()
    {
        if (currentTarget != null)
        {
            Outline outline = currentTarget.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            currentTarget = null;
        }
    }
}
