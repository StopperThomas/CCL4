using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform hingeBack;
    [SerializeField] private BoxLock boxLock;

    private bool isOpen = false;

    public void TryOpen()
    {
        if (isOpen) return;

        if (boxLock == null || boxLock.IsUnlocked())
        {
            isOpen = true;
            RotateHinge();
            Debug.Log("Box opened!");
        }
        else
        {
            Debug.Log("Box is still locked.");
            FindObjectOfType<InspectUIManager>()?.ShowPrompt(true, "The box is locked.", true);
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
            Debug.LogWarning("Hinge not assigned.");
        }
    }
}
