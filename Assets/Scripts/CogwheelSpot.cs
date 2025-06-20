using UnityEngine;

public class CogwheelSpot : MonoBehaviour
{
    public CogwheelType requiredType;
    private bool isOccupied = false;

    public bool TryPlaceEquippedCogwheel(Item item)
    {
        if (isOccupied)
        {
            Debug.Log("Spot already occupied.");
            return false;
        }

        if (item == null || item.itemType != ItemType.Cogwheel)
        {
            Debug.Log("Not a valid cogwheel.");
            return false;
        }

        if (item.cogwheelType != requiredType)
        {
            Debug.Log("Wrong cogwheel type.");
            return false;
        }



        GameObject cogwheelObj = Instantiate(item.prefab3D, transform.position, transform.rotation);
        cogwheelObj.transform.SetParent(transform);

          Rigidbody rb = cogwheelObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Disable interaction
        cogwheelObj.tag = "Untagged";
        Destroy(cogwheelObj.GetComponent<Collider>());

        // Ensure Cogwheel component is attached and configured
        Cogwheel cogComponent = cogwheelObj.GetComponent<Cogwheel>();
        if (cogComponent == null)
        {
            cogComponent = cogwheelObj.AddComponent<Cogwheel>();
        }
        cogComponent.size = item.cogwheelType;

        isOccupied = true;

        Debug.Log("Cogwheel placed successfully.");
        PuzzleDoor.Instance?.CheckPuzzleCompletion();

        return true;
    }


        public bool HasCorrectCogwheel()
    {
        if (!isOccupied) return false;

        Transform child = transform.GetChild(0); // assuming placed cogwheel is parented
        if (child == null) return false;

        Cogwheel cog = child.GetComponent<Cogwheel>();
        if (cog == null) return false;

        return cog.size == requiredType;
    }

    public bool IsOccupied() => isOccupied;
}