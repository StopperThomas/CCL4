using UnityEngine;

public class ScrewSocket : MonoBehaviour
{
    public ScrewType requiredType;

    private GameObject placedScrew;
    private bool isFilled = false;

    [SerializeField] private Vector3 screwOffset = new Vector3(-0.02f, -0.05f, 0f);
    [SerializeField] private AK.Wwise.Event screwPlaceSound;

    public bool IsFilled => isFilled;
    public ScrewType PlacedType { get; private set; }

    public bool TryPlaceScrew(GameObject screwPrefab, ScrewType type)
    {
        if (isFilled || screwPrefab == null)
            return false;

        if (type != requiredType)
        {
            PromptManager.Instance?.ShowPrompt("That screw doesn't fit here.");
            return false;
        }

        placedScrew = Instantiate(screwPrefab, transform.position, Quaternion.identity);
        placedScrew.transform.SetParent(transform);
        placedScrew.transform.localPosition = screwOffset;
        placedScrew.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        placedScrew.transform.localScale = Vector3.one;

        placedScrew.tag = "Untagged";

        Rigidbody rb = placedScrew.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        isFilled = true;
        PlacedType = type;

        screwPlaceSound?.Post(gameObject);

        PuzzleManager.Instance?.NotifyScrewPlaced(this);
        return true;
    }
}