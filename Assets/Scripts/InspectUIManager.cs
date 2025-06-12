using UnityEngine;
using TMPro;

public class InspectUIManager : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    // New method to show text and toggle visibility
    public void ShowPrompt(bool show, string message = "")
    {
        promptText.text = message;
        promptText.alpha = show ? 1f : 0f;
    }
}
