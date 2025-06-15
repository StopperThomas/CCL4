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
    }

    public void ShowItem(Item item)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;
        currentItem = item;

        if (dropHintText != null)
        {
            dropHintText.gameObject.SetActive(true);
            dropHintText.text = "Press Y to drop item";
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
            if (inspector != null)
                inspector.StartInventoryInspection(currentRender);

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
        if (item.prefab3D == null)
        {
            Debug.LogWarning("No prefab assigned to this item.");
            return;
        }

        Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        GameObject dropped = GameObject.Instantiate(item.prefab3D, dropPosition, Quaternion.identity);

        // If you want, tag it as interactable again:
        dropped.tag = "InvenontoryItem";

        InventoryManager.Instance.inventory.RemoveItem(item);
        InventoryManager.Instance.uiInventory.RefreshInventoryItems();

        HideItem();
        Debug.Log("🔁 Dropped item: " + item.itemName);
    }

    public void HideItem()
    {
        if (currentRender != null)
        {
            Destroy(currentRender);
            currentRender = null;
        }

        if (dropHintText != null)
        {
            dropHintText.text = "";
            dropHintText.gameObject.SetActive(false);
        }

        nameText.text = "";
        descriptionText.text = "";
        currentItem = null;
    }

    private void FitItemInView(GameObject obj)
    {
        if (renderCamera == null || obj == null) return;

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        Bounds bounds = renderer.bounds;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float padding = 0.8f;
        float distance = (maxSize * padding) / (2f * Mathf.Tan(Mathf.Deg2Rad * renderCamera.fieldOfView * 0.5f));

        obj.transform.rotation = Quaternion.LookRotation(-renderCamera.transform.forward);
        obj.transform.position = renderCamera.transform.position + renderCamera.transform.forward * distance;

        Vector3 centerOffset = renderer.bounds.center - obj.transform.position;
        obj.transform.position -= centerOffset;

        float scaleFactor = padding / maxSize;
        obj.transform.localScale = Vector3.one * scaleFactor;
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
}
