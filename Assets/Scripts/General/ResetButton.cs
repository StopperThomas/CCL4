using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public SwitchButton switchButton;

    private void OnMouseDown()
    {
        switchButton?.PressButton();
        TankPuzzleManager.Instance?.ResetPuzzle();
    }
}