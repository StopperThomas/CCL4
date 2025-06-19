using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform hingeBack;
    [SerializeField] private BoxLock boxLock;
    [SerializeField] private AK.Wwise.Event openSound; // Assign in Inspector

    private bool isOpen = false;

    public void TryOpen()
    {
        if (isOpen) return;

        if (boxLock == null || boxLock.IsUnlocked())
        {
            isOpen = true;

            if (openSound != null)
            {
                openSound.Post(gameObject);
            }

            RotateHinge();
            Debug.Log("Box opened!");
        }
        else
        {
            Debug.Log("Box is still locked.");
            PromptManager.Instance?.ShowPrompt("The box is locked.");
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
