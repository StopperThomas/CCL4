using UnityEngine;

public class ValveController : MonoBehaviour
{
    [Header("Valve Settings")]
    public float rotationAmount = 15f;            // How much the pointer turns
    public float valveSpinDuration = 0.5f;        // How long the valve visually spins

    [Header("References")]
    public PointerController pointerController;

    [SerializeField] private AK.Wwise.Event valveTurnSound; // Optional: Wwise sound

    private bool isSpinning = false;

    public void ActivateValve()
    {
        if (isSpinning || pointerController == null) return;

        // Prevent further interaction if already solved
        if (TankPuzzleManager.Instance != null && TankPuzzleManager.Instance.tankPuzzleSolved)
        {
            PromptManager.Instance?.ShowPrompt("The tank is already full.");
            return;
        }

        // Play valve sound
        if (valveTurnSound != null)
        {
            valveTurnSound.Post(gameObject);
        }

        StartCoroutine(SpinValveVisual());
        pointerController.RotateBy(rotationAmount);
        TankPuzzleManager.Instance.CheckIfInCorrectRange();
    }

    private System.Collections.IEnumerator SpinValveVisual()
    {
        isSpinning = true;

        float elapsed = 0f;
        float endAngle = 90f;

        Quaternion initialRotation = transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(endAngle, 0f, 0f);

        while (elapsed < valveSpinDuration)
        {
            float t = elapsed / valveSpinDuration;
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
        isSpinning = false;
    }
}