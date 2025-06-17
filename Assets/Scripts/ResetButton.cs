using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public SwitchButton switchButton;

    private void OnMouseDown()
    {
        switchButton?.PressButton(); // Play the visual animation
        TankPuzzleManager.Instance?.ResetPuzzle(); // Logic reset
    }
}

