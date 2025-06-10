using UnityEngine;
using TMPro;

public class InspectUIManager : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    public void ShowPrompt(bool show)
    {
        promptText.alpha = show ? 1f : 0f;
    }
}
