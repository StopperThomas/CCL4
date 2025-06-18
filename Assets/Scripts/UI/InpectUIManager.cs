using UnityEngine;
using TMPro;
using System.Collections;

public class InspectUIManager : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    private Coroutine hidePromptRoutine;

    public void ShowPrompt(bool show, string message = "", bool autoHide = false)
    {
        if (hidePromptRoutine != null)
        {
            StopCoroutine(hidePromptRoutine);
            hidePromptRoutine = null;
        }

        if (promptText == null) return;

        promptText.text = message;
        promptText.alpha = show ? 1f : 0f;

        if (show && autoHide)
        {
            hidePromptRoutine = StartCoroutine(HidePromptAfterDelay(2f));
        }
    }

    private IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        promptText.alpha = 0f;
        hidePromptRoutine = null;
    }
}
