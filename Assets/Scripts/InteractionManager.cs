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

    void OnEnable() => controls?.Enable();
    void OnDisable() => controls?.Disable();

    void Update()
    {
        if (isInspecting) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
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

        // Scene change
        if (currentTarget.CompareTag("SceneChanger"))
        {
            var transition = currentTarget.GetComponent<SceneTransitionObject>();
            transition?.LoadScene();
            return;
        }

        // Handle Screw-specific logic
        var screw = currentTarget.GetComponent<Screw>();
        if (screw != null && !screw.isUnscrewed)
        {
            PromptManager.Instance?.ShowPrompt("You need to unscrew it first.");
            return; // Block inspection
        }

        // Regular interactable or inventory item
        if (currentTarget.CompareTag("Interactable") || currentTarget.CompareTag("InventoryItem"))
        {
            isInspecting = true;
            inspector.StartInspection(currentTarget);
            if (playerController != null) playerController.enabled = false;
        }
    }


    void TryClick()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance)) return;

        GameObject hitObject = hit.collider.gameObject;

        // Lightbulb placement
        var fixture = hitObject.GetComponent<Fixture>();
        if (fixture != null && equippedItem != null && equippedItem.itemType == ItemType.LightBulb)
        {
            if (!PuzzleManager.Instance.IsEnginePuzzleSolved)
            {
                PromptManager.Instance?.ShowPrompt("There's no power yet. Check the sides if the screws are placed.");
                return;
            }

            if (!InventoryManager.Instance.inventory.GetItemList().Contains(equippedItem))
            {
                PromptManager.Instance?.ShowPrompt("This bulb is no longer in your inventory.");
                UnequipItem();
                return;
            }

            bool placed = fixture.TryPlaceBulb(equippedItem.GetPrefab());
            if (placed)
            {
                InventoryManager.Instance.inventory.RemoveItem(equippedItem); // handles amount or full removal
                PromptManager.Instance?.ShowPrompt("You placed the bulb.");

                if (equippedItem.amount <= 0)
                    UnequipItem();
                else
                    InventoryManager.Instance.uiInventory.RefreshInventoryItems();
            }

            return;
        }

        // Valve interaction
        ValveController valve = hitObject.GetComponent<ValveController>();
        if (valve != null)
        {
            valve.ActivateValve();
            PromptManager.Instance?.ShowPrompt("You turned the valve.");
            return;
        }

        var drawer = hitObject.GetComponent<DeskDrawer>();
        if (drawer != null)
        {
            drawer.TryOpen(equippedItem);
            return;
        }

        // Screw placement
        var screwSocket = hitObject.GetComponent<ScrewSocket>();
        if (screwSocket != null && equippedItem != null && equippedItem.itemType == ItemType.Screw)
        {
            if (equippedItem.amount <= 0)
            {
                PromptManager.Instance?.ShowPrompt("No screws left.");
                UnequipItem();
                return;
            }

            bool placed = screwSocket.TryPlaceScrew(equippedItem.GetPrefab(), equippedItem.screwType);

            if (placed)
            {
                equippedItem.amount--;

                if (equippedItem.amount <= 0)
                {
                    InventoryManager.Instance.inventory.RemoveItem(equippedItem);
                    UnequipItem();
                }
                else
                {
                    InventoryManager.Instance.uiInventory.RefreshInventoryItems();
                }

                PromptManager.Instance?.ShowPrompt("Screw placed.");
            }
            else
            {
                PromptManager.Instance?.ShowPrompt("That screw doesn't fit here.");
            }

            return;
        }

        var screw = hitObject.GetComponent<Screw>();
        if (screw != null)
        {
            if (PuzzleManager.Instance != null && PuzzleManager.Instance.IsEnginePuzzleSolved)
            {
                PromptManager.Instance?.ShowPrompt("The circuit is already completed.");
                return;
            }

            if (!screw.isUnscrewed)
            {
                if (equippedItem != null &&
                    equippedItem.itemType == ItemType.ScrewDriver &&
                    screw.TryUnscrew(equippedItem))
                {
                    PromptManager.Instance?.ShowPrompt("You unscrewed it.");
                }
                else
                {
                    PromptManager.Instance?.ShowPrompt("That won't work here.");
                }
            }
            else
            {
                var pickup = hitObject.GetComponent<PickupItem>();
                pickup?.OnPickup();
                PromptManager.Instance?.ShowPrompt("Item picked up.");
            }

            return;
        }

        var pickupItem = hitObject.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            pickupItem.OnPickup();
            PromptManager.Instance?.ShowPrompt("Item picked up.");
            return;
        }

        // Cogwheel pickup
        var cog = hitObject.GetComponent<Cogwheel>();
        if (cog != null && !CogwheelManager.Instance.IsHoldingCogwheel())
        {
            CogwheelManager.Instance.PickUpCogwheel(cog);
            cog.gameObject.SetActive(false);
            PromptManager.Instance?.ShowPrompt("Picked up a cogwheel.");
            return;
        }

        // Cogwheel placement
        var spot = hitObject.GetComponent<CogwheelSpot>();
        if (spot != null && equippedItem != null && equippedItem.itemType == ItemType.Cogwheel)
        {
            bool placed = spot.TryPlaceEquippedCogwheel(equippedItem);
            if (placed)
            {
                equippedItem.amount--;

                if (equippedItem.amount <= 0)
                {
                    InventoryManager.Instance.inventory.RemoveItem(equippedItem);
                    UnequipItem();
                }
                else
                {
                    InventoryManager.Instance.uiInventory.RefreshInventoryItems();
                }

                PromptManager.Instance?.ShowPrompt("Cogwheel placed.");
            }
            return;
        }

        // Book selection
        var book = hitObject.GetComponent<BookSelectable>();
        if (book != null)
        {
            book.ToggleSelection();
            return;
        }

        // Unlock
        var lockComponent = hitObject.GetComponent<BoxLock>();
        lockComponent?.TryUnlock(equippedItem);

        // Open box
        var box = hitObject.GetComponent<Box>();
        box?.TryOpen();
    }


    void TryPickupItem(GameObject target)
    {
        if (!target.CompareTag("InventoryItem")) return;

        var pickup = target.GetComponent<PickupItem>();
        pickup?.OnPickup();
        ExitInspectMode();
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        inspector.EndInspection();
        if (playerController != null)
            playerController.enabled = true;
    }

    public void ForceExitInspectMode()
    {
        if (isInspecting)
            ExitInspectMode();
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
        var outline = obj.GetComponent<Outline>();
        if (outline != null)
            outline.enabled = enable;
    }

    public void EquipFromUI(Item item)
    {
        if (item == null ||
            (item.itemType != ItemType.ScrewDriver &&
             item.itemType != ItemType.Key &&
             item.itemType != ItemType.Screw &&
             item.itemType != ItemType.Cogwheel &&
             item.itemType != ItemType.LightBulb))
        {
            Debug.LogWarning("Tried to equip invalid item.");
            return;
        }

        if (!InventoryManager.Instance.inventory.GetItemList().Contains(item))
        {
            Debug.LogWarning("Item not in inventory anymore.");
            return;
        }

        equippedItem = item;
        Debug.Log("Equipped item in interaction manager: " + item.itemName);
    }

    public Item GetEquippedItem() => equippedItem;

    public void UnequipItem()
    {
        if (equippedItem == null)
        {
            Debug.Log("No item equipped to unequip.");
            return;
        }

        var inventory = InventoryManager.Instance?.inventory;

        if (inventory != null && !inventory.GetItemList().Contains(equippedItem) && equippedItem.amount > 0)
        {
            inventory.AddItem(equippedItem);
        }

        Debug.Log("Unequipped item: " + equippedItem.itemName);
        equippedItem = null;

        InventoryManager.Instance?.uiInventory?.UpdateEquippedSlot(null);
    }
}
