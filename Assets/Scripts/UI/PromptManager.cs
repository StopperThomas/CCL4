using UnityEngine;
using TMPro;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    public GameObject promptPanel;
    public TextMeshProUGUI promptText;
    public KeyCode defaultContinueKey = KeyCode.Space;

    private System.Action onConfirm;
    private bool waitingForInput = false;

    private float promptCooldown = 0.5f;
    private float lastPromptTime = -999f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowPrompt(string message, System.Action onConfirmed = null)
    {
        if (Time.time - lastPromptTime < promptCooldown)
        {
            Debug.Log($"Prompt blocked: '{message}' (too soon)");
            return;
        }

        lastPromptTime = Time.time;

        Debug.Log($"ShowPrompt called with message: {message}");
        if (promptText != null)
            promptText.text = message;

        if (promptPanel != null)
            promptPanel.SetActive(true);

        onConfirm = onConfirmed;
        waitingForInput = true;
    }

    private void Update()
    {
        if (waitingForInput && Input.GetKeyDown(defaultContinueKey))
        {
            promptPanel.SetActive(false);
            waitingForInput = false;
            onConfirm?.Invoke();
        }
    }
}