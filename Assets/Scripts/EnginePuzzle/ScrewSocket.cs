using UnityEngine;

public class ScrewSocket : MonoBehaviour
{
    public ScrewType requiredType;

    private GameObject placedScrew;
    private bool isFilled = false;

    [SerializeField] private Vector3 screwOffset = new Vector3(-0.02f, -0.05f, 0f);


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

    // Instantiate and parent the screw to this socket
    placedScrew = Instantiate(screwPrefab, transform.position, Quaternion.identity);
    placedScrew.transform.SetParent(transform);

    // Apply local offset and rotation
    placedScrew.transform.localPosition = screwOffset;
    placedScrew.transform.localRotation = Quaternion.Euler(180f, 0f, 0f); // Flip for visual alignment
    placedScrew.transform.localScale = Vector3.one;

    // Disable further interaction
    placedScrew.tag = "Untagged";

    // Disable gravity and physics
    Rigidbody rb = placedScrew.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    isFilled = true;
    PlacedType = type;

    // Notify the puzzle manager
    PuzzleManager.Instance?.NotifyScrewPlaced(this);

    return true;
}




}
