using UnityEngine;

public class Cogwheel : MonoBehaviour
{
    public Cogwheel size;
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
