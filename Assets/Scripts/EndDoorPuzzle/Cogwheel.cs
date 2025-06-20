using UnityEngine;

public class Cogwheel : MonoBehaviour
{
    public CogwheelType size;  
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