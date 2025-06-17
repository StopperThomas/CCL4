using UnityEngine;

public class LightBulb : MonoBehaviour
{
    public bool isPowered = false;
    public bool canPickup = true;

    private Renderer bulbRenderer;    
    private Color originalColor;     

    public Color poweredColor = Color.yellow; 
    private void Awake()
    {
        if (bulbRenderer == null)
        {
            Transform bulbPart = transform.Find("Bulb");
            if (bulbPart != null)
                bulbRenderer = bulbPart.GetComponent<Renderer>();

            if (bulbRenderer != null)
                originalColor = bulbRenderer.material.color;
            else
                Debug.LogWarning("Could not find 'Bulb' mesh to assign renderer.");
        }
        else
        {
            originalColor = bulbRenderer.material.color;
        }
    }

    public void SetPowered(bool powered)
    {
        isPowered = powered;
        canPickup = !powered;

        Light light = GetComponentInChildren<Light>();
        if (light != null)
            light.enabled = powered;

        if (bulbRenderer != null)
        {
            bulbRenderer.material.color = powered ? poweredColor : originalColor;
        }
        if (powered)
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
    }
}
