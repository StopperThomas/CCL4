using UnityEngine;

public class Cogwheel : MonoBehaviour
{
    public CogwheelType size;  // ✅ Corrected to the enum type
    private bool isPlaced = false;

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}
