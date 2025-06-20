using UnityEngine;

public class CombinationLockDigit : MonoBehaviour
{
    public int digitIndex; // 0 = left, 1 = middle, 2 = right
    public CombinationLockController lockController;

    private int currentValue = 0;

    public void Interact()
    {
        RotateDigit();
    }

    void RotateDigit()
    {
        currentValue = (currentValue + 1) % 10;
        transform.Rotate(Vector3.up, 36f);
        lockController.UpdateDigit(digitIndex, currentValue);
    }

    public void SetValue(int value)
    {
        currentValue = value % 10;
    }

    public int GetValue()
    {
        return currentValue;
    }
}