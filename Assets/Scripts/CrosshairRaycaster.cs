using UnityEngine;

public class CrosshairRaycaster : MonoBehaviour
{
    public float rayDistance = 5f;
    private GameObject currentTarget;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (hit.collider.gameObject != currentTarget)
                {
                    ClearHighlight();
                    currentTarget = hit.collider.gameObject;
                    ApplyHighlight(currentTarget);
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
