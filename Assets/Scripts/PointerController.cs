using UnityEngine;

public class PointerController : MonoBehaviour
{
    [Range(0f, 360f)] public float currentAngle = 0f;
    public Transform pointerTransform;

    public void RotateBy(float angle)
    {
        currentAngle = (currentAngle + angle) % 360f;

        if (pointerTransform != null)
            pointerTransform.localRotation = Quaternion.Euler(-currentAngle, 0, 0);
    }

    public void ResetPointer()
    {
        currentAngle = 0f;

        if (pointerTransform != null)
            pointerTransform.localRotation = Quaternion.identity;
    }
}
