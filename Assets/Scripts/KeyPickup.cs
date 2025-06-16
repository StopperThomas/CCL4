using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool HasKey = false;

    public void PickUp()
    {
        HasKey = true;
        gameObject.SetActive(false);
        Debug.Log("Key picked up!");
    }
}
