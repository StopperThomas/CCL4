using UnityEngine;

public class CrosshairRaycaster : MonoBehaviour
{
    public float rayDistance = 5f;
    public InteractionManager interactionManager;
    public InspectUIManager uiManager;

    [SerializeField] private AK.Wwise.Event clickSoundEvent; // Assign in Inspector via Wwise picker

    private GameObject lastHighlighted;

    void Start()
    {
        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<InteractionManager>();
            if (interactionManager == null)
                Debug.LogError("InteractionManager not found!");
        }

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<InspectUIManager>();
            if (uiManager == null)
                Debug.LogWarning("InspectUIManager not assigned!");
        }
    }

    void Update()
    {
        if (interactionManager == null) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObj = hit.collider.gameObject;

            BoxLock boxLock = hitObj.GetComponent<BoxLock>();
            if (boxLock != null)
            {
                var equipped = interactionManager.GetEquippedItem();
                if (equipped != null && equipped.itemType == ItemType.Key)
                {
                    Highlight(hitObj);
                    uiManager?.ShowPrompt(true, "LMB");
                }
                else
                {
                    RemoveHighlight();
                    uiManager?.ShowPrompt(false);
                }
                return;
            }

            Screw screw = hitObj.GetComponent<Screw>();
            if (screw != null)
            {
                var equipped = interactionManager.GetEquippedItem();
                if (equipped != null && equipped.itemType == ItemType.ScrewDriver)
                {
                    Highlight(hitObj);
                    uiManager?.ShowPrompt(true, "LMB");
                }
                else
                {
                    RemoveHighlight();
                    uiManager?.ShowPrompt(false);
                }
                return;
            }

            if (hitObj.CompareTag("InventoryItem"))
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "E");
                return;
            }

            CombinationLockDigit digit = hitObj.GetComponent<CombinationLockDigit>();
            if (digit != null)
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "LMB");

                if (Input.GetMouseButtonDown(0))
                {
                    PlayClickSound(hitObj);
                    digit.Interact();
                }

                return;
            }

            if (hitObj.CompareTag("Book"))
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "LMB");
                return;
            }

            Potion potion = hitObj.GetComponent<Potion>();
            if (potion != null)
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "LMB");

                if (Input.GetMouseButtonDown(0))
                {
                    PlayClickSound(hitObj);
                    potion.FlyToCauldron();
                }
                return;
            }

            Sugar sugar = hitObj.GetComponent<Sugar>();
            if (sugar != null)
            {
                Highlight(hitObj);
                uiManager?.ShowPrompt(true, "LMB");

                if (Input.GetMouseButtonDown(0))
                {
                    PlayClickSound(hitObj);
                    sugar.OnInteract();
                }
                return;
            }

            RemoveHighlight();
            uiManager?.ShowPrompt(false);
        }
        else
        {
            RemoveHighlight();
            uiManager?.ShowPrompt(false);
        }
    }

    void Highlight(GameObject obj)
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

    void PlayClickSound(GameObject sourceObj)
    {
        if (clickSoundEvent != null)
        {
            clickSoundEvent.Post(sourceObj);
        }
        else
        {
            Debug.LogWarning("Wwise click event not assigned.");
        }
    }
}
