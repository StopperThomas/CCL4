using UnityEngine;

public class CogwheelSpot : MonoBehaviour
{
    public CogwheelSize requiredSize;

    private bool isOccupied = false;
    private Cogwheel placedCogwheel;

    public bool IsCorrectlyFilled
    {
        get { return isOccupied && placedCogwheel != null && placedCogwheel.size == requiredSize; }
    }


    public bool TryPlaceCogwheel(Cogwheel cogwheel)
    {
        if (isOccupied)
        {
            Debug.Log("Spot already has a cogwheel.");
            return false;
        }

        if (cogwheel.size != requiredSize)
        {
            Debug.Log($"Wrong cogwheel size. Needed: {requiredSize}, but got: {cogwheel.size}");
            return false;
        }

        isOccupied = true;
        placedCogwheel = cogwheel;

        cogwheel.transform.position = transform.position;
        cogwheel.transform.rotation = transform.rotation;
        cogwheel.transform.SetParent(transform);
        cogwheel.gameObject.SetActive(true);

        Collider cogCollider = cogwheel.GetComponent<Collider>();
        if (cogCollider) cogCollider.enabled = false;

        CogwheelManager.Instance.ClearHeldCogwheel();

        Debug.Log("Cogwheel placed successfully.");

        PuzzleDoor.Instance.CheckPuzzleCompletion();

        return true;
    }

    public bool HasCorrectCogwheel()
    {
        return isOccupied && placedCogwheel != null && placedCogwheel.size == requiredSize;
    }
}
