using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public float rayDistance = 5f;

    public ObjectInspector inspector;
    public PlayerController playerController;

    private GameObject currentTarget;
    private bool isInspecting = false;

    private PlayerInputActions controls;
    private Item equippedItem;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.Player.Inspect.performed += ctx =>
        {
            if (isInspecting)
                ExitInspectMode();
            else
                TryInspect();
        };

        controls.Player.Cancel.performed += ctx =>
        {
            if (isInspecting)
                ExitInspectMode();
        };

        controls.Player.Click.performed += ctx =>
        {
            if (!isInspecting)
                TryClick();
        };

        controls.Player.AddToInventory.performed += ctx =>
        {
            if (isInspecting && currentTarget != null)
                TryPickupItem(currentTarget);
        };
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (isInspecting) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Interactable") ||
    hitObject.CompareTag("InventoryItem") ||
    hitObject.CompareTag("SceneChanger"))
            {
                if (hitObject != currentTarget)
                {
                    ClearHighlight();
                    currentTarget = hitObject;
                    EnableOutline(currentTarget, true);
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void TryInspect()
    {
        if (currentTarget == null || isInspecting) return;

        if (currentTarget.CompareTag("SceneChanger"))
        {
            SceneTransitionObject transition = currentTarget.GetComponent<SceneTransitionObject>();
            if (transition != null)
            {
                transition.LoadScene();
                return;
            }
        }

        if (currentTarget.CompareTag("Interactable") || currentTarget.CompareTag("InventoryItem"))
        {
            isInspecting = true;
            inspector.StartInspection(currentTarget);
            if (playerController != null) playerController.enabled = false;
        }

        Screw screw = currentTarget.GetComponent<Screw>();
        if (screw != null)
        {
            screw.TryUnscrew(equippedItem);
            return;
        }
    }

    void TryClick()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            Screw screw = hitObject.GetComponent<Screw>();
            if (screw != null)
            {
                screw.TryUnscrew(equippedItem);
                return;
            }


        }
    }

    void TryPickupItem(GameObject target)
    {
        if (!target.CompareTag("InventoryItem")) return;

        PickupItem pickup = target.GetComponent<PickupItem>();
        if (pickup != null)
        {
            pickup.OnPickup();
            ExitInspectMode();
        }
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        inspector.EndInspection();

        if (playerController != null)
            playerController.enabled = true;
    }

    void ClearHighlight()
    {
        if (currentTarget != null)
        {
            EnableOutline(currentTarget, false);
            currentTarget = null;
        }
    }

    void EnableOutline(GameObject obj, bool enable)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable;
        }
    }

    public void EquipFromUI(Item item)
    {
        if (item.itemType != Item.ItemType.ScrewDriver)
        {
            Debug.LogWarning("Tried to equip non-screwdriver item.");
            return;
        }

        equippedItem = item;
        Debug.Log("Equipped screwdriver: " + item.itemName + " Type: " + item.screwType);
    }

    public Item GetEquippedItem()
    {
        return equippedItem;
    }
    public void UnequipItem()
    {
        if (equippedItem == null)
        {
            Debug.Log("No item equipped to unequip.");
            return;
        }


        Inventory inventory = InventoryManager.Instance?.inventory;
        if (inventory != null && !inventory.GetItemList().Contains(equippedItem))
        {
            inventory.AddItem(equippedItem);
        }

        Debug.Log("Unequipped item: " + equippedItem.itemName);

        equippedItem = null;


        if (InventoryManager.Instance != null && InventoryManager.Instance.uiInventory != null)
        {
            InventoryManager.Instance.uiInventory.UpdateEquippedSlot(null);
        }
    }

}
