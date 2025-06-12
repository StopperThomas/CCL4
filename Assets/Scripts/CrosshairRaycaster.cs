using UnityEngine;

public class CrosshairRaycaster : MonoBehaviour
{
    public float rayDistance = 5f;
    public InspectUIManager uiManager;

    private GameObject currentTarget;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObj = hit.collider.gameObject;

            bool isInteractable = hitObj.CompareTag("Interactable");
            bool isScrew = hitObj.GetComponent<Screw>() != null;
            bool isScrewdriver = hitObj.CompareTag("Screwdriver");
            bool isDoor = hitObj.CompareTag("SceneChanger");

            if (isInteractable || isScrew || isScrewdriver || isDoor)
            {
                if (hitObj != currentTarget)
                {
                    ClearHighlight();
                    currentTarget = hitObj;
                    ApplyHighlight(currentTarget);
                }

                // Show prompt based on type
                if (isInteractable || isDoor)
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
