using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInspectorUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Transform renderAnchor;
    public Camera renderCamera;

    private GameObject currentRender;

    public void ShowItem(Item item)
    {
        nameText.text = item.itemName;
        descriptionText.text = item.description;

        if (currentRender != null)
            Destroy(currentRender);

        if (item.prefab3D != null && renderAnchor != null && renderCamera != null)
        {
            currentRender = Instantiate(item.prefab3D, renderAnchor);
            currentRender.transform.localPosition = Vector3.zero;
            currentRender.transform.localRotation = Quaternion.identity;
            currentRender.transform.localScale = Vector3.one;


            int renderLayer = LayerMask.NameToLayer("ItemRenderLayer");
            if (renderLayer == -1)
            {
                Debug.LogWarning("⚠️ 'ItemRenderLayer' does not exist. Skipping layer assignment.");
            }
            else
            {
                SetLayerRecursively(currentRender, renderLayer);
            }

            // Position item in front of the render camera
            Vector3 offset = renderCamera.transform.forward * 1.5f;
            currentRender.transform.position = renderCamera.transform.position + offset;
            currentRender.transform.rotation = Quaternion.identity;

            // Optionally start rotation (if using ObjectInspector)
            ObjectInspector inspector = FindObjectOfType<ObjectInspector>();
            if (inspector != null)
            {
                inspector.StartInventoryInspection(currentRender);
            }

            Debug.Log("✅ Item shown: " + item.itemName);
        }
        else
        {
            Debug.LogWarning("Missing prefab3D, renderAnchor, or renderCamera for item: " + item.itemName);
        }
        
        FitItemInView(currentRender);
    }

    private void FitItemInView(GameObject obj)
{
    if (renderCamera == null) return;

    Renderer renderer = obj.GetComponentInChildren<Renderer>();
    if (renderer == null) return;

    Bounds bounds = renderer.bounds;

    // Calculate the max dimension
    float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

    // Distance from camera to fit the object based on FOV
    float distance = maxSize / (2f * Mathf.Tan(Mathf.Deg2Rad * renderCamera.fieldOfView * 0.5f));

    // Position the object directly in front of the camera
    obj.transform.position = renderCamera.transform.position + renderCamera.transform.forward * distance;

    // Optionally center the object
    Vector3 offset = bounds.center - obj.transform.position;
    obj.transform.position -= offset;

    // Scale down if necessary
    float scaleFactor = 1f / maxSize;
    obj.transform.localScale = Vector3.one * scaleFactor;
}


    public void HideItem()
    {
        if (currentRender != null)
        {
            Destroy(currentRender);
            currentRender = null;
        }
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
