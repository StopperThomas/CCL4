using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public float pressDistance = 0.05f; // How far it moves when pressed
    public float pressDuration = 0.1f;  // How fast it presses/rebounds

    private Vector3 initialPosition;
    private bool isAnimating = false;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void PressButton()
    {
        if (!isAnimating)
            StartCoroutine(PressAndRelease());
    }

    private System.Collections.IEnumerator PressAndRelease()
    {
        isAnimating = true;

        // Target pressed position
        Vector3 pressedPosition = initialPosition - new Vector3(pressDistance, 0, 0);

        // Press in
        float elapsed = 0f;
        while (elapsed < pressDuration)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, pressedPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = pressedPosition;

        // Pause briefly (optional)
        yield return new WaitForSeconds(0.05f);

        // Return back
        elapsed = 0f;
        while (elapsed < pressDuration)
        {
            transform.localPosition = Vector3.Lerp(pressedPosition, initialPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;

        isAnimating = false;
    }
}