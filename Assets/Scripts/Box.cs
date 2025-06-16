using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform hingeBack;
    [SerializeField] private BoxLock boxLock;

    private bool isOpen = false;

    public void TryOpen()
    {
        if (isOpen)
            return;

        if (boxLock == null || boxLock.IsUnlocked())
        {
            isOpen = true;
            RotateHinge();
            Debug.Log("Box opened!");
        }
        else
        {
            Debug.Log("Box is still locked.");
        }
    }

    private void RotateHinge()
    {
        if (hingeBack != null)
        {
            hingeBack.localRotation = Quaternion.Euler(-45f, 0f, 0f);
        }
        else
        {
            Debug.LogWarning("HingeBack not assigned!");
        }
    }
}
