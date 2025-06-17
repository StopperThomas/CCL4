using UnityEngine;

public class ResetButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        TankPuzzleManager.Instance?.ResetPuzzle();
    }
}
