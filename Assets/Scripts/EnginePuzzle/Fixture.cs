using UnityEngine;

public class Fixture : MonoBehaviour
{
    public bool isCorrectSocket; 
    private GameObject placedBulb;

    public bool TryPlaceBulb(GameObject bulbPrefab)
    {
        if (placedBulb != null || bulbPrefab == null) return false;

        Quaternion prefabRotation = bulbPrefab.transform.rotation;
        Vector3 placementPosition = transform.position + transform.up * 0.03f;

        placedBulb = Instantiate(bulbPrefab, placementPosition, prefabRotation);
        placedBulb.transform.localScale = Vector3.one;

        if (isCorrectSocket)
        {
            PowerBulb();
        }
        else
        {
            Debug.Log("Wrong socket. Bulb placed, but no power.");
        }

        return true;
    }

    private void PowerBulb()
{
    LightBulb bulbScript = placedBulb.GetComponent<LightBulb>();
    if (bulbScript != null)
    {
        bulbScript.SetPowered(true);
        Debug.Log("Bulb powered via LightBulb script.");
    }
    else
    {
        Debug.LogWarning("No LightBulb script found on placed bulb!");
    }

    // Change color to yellow
    Renderer rend = placedBulb.GetComponentInChildren<Renderer>();
    if (rend != null)
    {
        Material matInstance = new Material(rend.material);
        matInstance.color = Color.yellow;
        rend.material = matInstance;
    }

    // Lock interaction
    placedBulb.tag = "Untagged";


}

    public void RemoveBulb()
    {
        if (placedBulb != null)
        {
            Destroy(placedBulb);
            placedBulb = null;
        }
    }
}
