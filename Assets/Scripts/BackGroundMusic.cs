using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Wwise Music Settings")]
    [SerializeField] private AK.Wwise.Event musicEvent;   // Drop your music event here (e.g. MainMenuMusic or GameplayMusic)
    [SerializeField] private AK.Wwise.Bank musicBank;     // Optional: assign the bank if you're manually loading it

    private void Start()
    {
        if (musicBank != null)
        {
            musicBank.Load();
        }

        if (musicEvent != null)
        {
            musicEvent.Post(gameObject);
        }
    }

    private void OnDisable()
    {
        AkUnitySoundEngine.StopAll(gameObject); // Stop music when the GameObject is disabled or scene changes
    }
}
