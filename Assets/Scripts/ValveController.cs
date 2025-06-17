using UnityEngine;

public class ValveController : MonoBehaviour
{
    [Header("Valve Settings")]
    public float rotationAmount = 15f; // How much the pointer turns
    public float valveSpinDuration = 0.5f; // How long the valve visually spins

    [Header("References")]
    public PointerController pointerController;

    private bool isSpinning = false;

  public void ActivateValve()
{
    if (isSpinning || pointerController == null) return;

    StartCoroutine(SpinValveVisual());
    pointerController.RotateBy(rotationAmount);
    TankPuzzleManager.Instance.CheckIfInCorrectRange();
}
   private System.Collections.IEnumerator SpinValveVisual()
{
    isSpinning = true;

    float elapsed = 0f;
    float endAngle = 90f;

    // Cache the initial local rotation
    Quaternion initialRotation = transform.localRotation;
    Quaternion targetRotation = initialRotation * Quaternion.Euler(endAngle, 0f, 0f);

    while (elapsed < valveSpinDuration)
    {
        float t = elapsed / valveSpinDuration;
        transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);

        elapsed += Time.deltaTime;
        yield return null;
    }

    // Snap to final rotation
    transform.localRotation = targetRotation;
    isSpinning = false;
}


}
