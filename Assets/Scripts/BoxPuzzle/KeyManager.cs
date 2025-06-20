using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    private bool hasKey = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnKeyCollected()
    {
        hasKey = true;
        Debug.Log("Key collected via KeyManager");
    }

    public bool HasKey() => hasKey;
}