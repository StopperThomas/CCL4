using UnityEngine;

public class BoxLock : MonoBehaviour
{
    private bool isUnlocked = false;

    public void TryUnlock()
    {
        if (KeyPickup.HasKey && !isUnlocked)
        {
            isUnlocked = true;
            gameObject.SetActive(false); // Hide lock
            Debug.Log("Lock removed!");
        }
        else if (!KeyPickup.HasKey)
        {
            Debug.Log("You need the key!");
        }
    }

    public bool IsUnlocked() => isUnlocked;
}
