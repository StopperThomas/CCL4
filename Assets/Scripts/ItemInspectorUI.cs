using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ItemInspectorUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI dropHintText;
    public Transform renderAnchor;
    public Camera renderCamera;

    private GameObject currentRender;
    private Item currentItem;

    private void OnEnable()
    {
        if (dropHintText != null)
            dropHintText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            TryDropItemInput();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryEquipInspectedItem();
        }
    }

    public void ShowItem(Item item)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;
        currentItem = item;

        if (dropHintText != null)
        {
            dropHintText.text = "Press Y to drop item";
            dropHintText.gameObject.SetActive(true);
        }

        if (currentRender != null)
            Destroy(currentRender);

        if (item.prefab3D != null && renderAnchor != null && renderCamera != null)
        {
            currentRender = Instantiate(item.prefab3D, renderAnchor);
            currentRender.transform.localPosition = Vector3.zero;
            currentRender.transform.localRotation = Quaternion.identity;
            currentRender.transform.localScale = Vector3.one;

            int renderLayer = LayerMask.NameToLayer("ItemRenderLayer");
            if (renderLayer != -1)
                SetLayerRecursively(currentRender, renderLayer);

            FitItemInView(currentRender);

            ObjectInspector inspector = FindObjectOfType<ObjectInspector>();
            inspector?.StartInventoryInspection(currentRender);

            Debug.Log("Item shown: " + item.itemName);
        }
        else
        {
            Debug.LogWarning("Missing prefab3D, renderAnchor, or renderCamera for item: " + item.itemName);
        }
    }

    public void TryDropItemInput()
    {
        if (currentItem != null)
        {
            DropItem(currentItem);
        }
    }

    private void DropItem(Item item)
    {
        if (item == null || item.prefab3D == null)
            return;


        Item actualInventoryItem = InventoryManager.Instance.inventory.GetItemList().Find(i => i == item);
        if (actualInventoryItem == null)
        {
            Debug.LogWarning("Item not found in inventory.");
            return;
        }

        Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        GameObject dropped = Instantiate(item.prefab3D, dropPosition, Quaternion.identity);
        dropped.tag = "InventoryItem";

        InventoryManager.Instance.inventory.RemoveItem(actualInventoryItem);
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();

        HideItem();
    }

    public void HideItem()
    {
        if (currentRender != null)
        {
            Destroy(currentRender);
            currentRender = null;
        }

        if (renderAnchor != null)
        {
            foreach (Transform child in renderAnchor)
            {
                Destroy(child.gameObject);
            }
        }

        if (dropHintText != null)
        {
            dropHintText.text = "";
            dropHintText.gameObject.SetActive(false);
        }

        if (nameText != null)
        {
            nameText.text = "";
        }

        if (descriptionText != null)
        {
            descriptionText.text = "";
        }

        currentItem = null;

        Debug.Log("ItemInspectorUI: Cleared all item data.");
    }



    public void TryEquipInspectedItem()
    {
        if (currentItem != null && IsEquipable(currentItem))
        {
            var ui = FindObjectOfType<UI_Inventory>();
            ui?.UpdateEquippedSlot(currentItem);

            var interaction = FindObjectOfType<InteractionManager>();
            interaction?.EquipFromUI(currentItem);

            Debug.Log("Equipped from inspector: " + currentItem.itemName);
        }
    }

    private bool IsEquipable(Item item)
    {
        return item.itemType == ItemType.ScrewDriver ||
               item.itemType == ItemType.Key ||
               item.itemType == ItemType.Screw ||
               item.itemType == ItemType.Cogwheel ||
               item.itemType == ItemType.LightBulb;
    }
    private void FitItemInView(GameObject obj)
{
    if (renderCamera == null || obj == null) return;

    Renderer renderer = obj.GetComponentInChildren<Renderer>();
    if (renderer == null) return;

    Bounds bounds = renderer.bounds;
    float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
    float padding = 0.8f;

    float distance;

    if (currentItem != null && currentItem.customRenderDistance > 0f)
    {
        distance = currentItem.customRenderDistance;
    }
    else
    {
        distance = (maxSize * padding) / (2f * Mathf.Tan(Mathf.Deg2Rad * renderCamera.fieldOfView * 0.5f));
    }

    obj.transform.rotation = Quaternion.LookRotation(-renderCamera.transform.forward);
    obj.transform.position = renderCamera.transform.position + renderCamera.transform.forward * distance;

    // Centering the object
    Vector3 centerOffset = renderer.bounds.center - obj.transform.position;
    obj.transform.position -= centerOffset;

    float scale = currentItem != null ? currentItem.customRenderScale : 1f;
    obj.transform.localScale = Vector3.one * scale;
}


    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            if (child != null)
                SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public Item GetCurrentItem()
    {
        return currentItem;
    }

}
