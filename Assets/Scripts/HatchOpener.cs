using UnityEngine;

public class HatchOpener : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Transform hingePoint;           // Assign in inspector
    public Vector3 rotationAxis = Vector3.right;
    public float openAngle = 90f;
    public float openSpeed = 45f;

    private float currentAngle = 0f;
    private bool isOpening = false;

    void Update()
    {
        if (isOpening && currentAngle < openAngle)
        {
            float delta = openSpeed * Time.deltaTime;
            float remaining = openAngle - currentAngle;

            if (delta > remaining)
                delta = remaining;

            if (hingePoint != null)
            {
                transform.RotateAround(hingePoint.position, hingePoint.TransformDirection(rotationAxis), delta);
            }
            else
            {
                Debug.LogWarning("Hinge point not assigned on HatchOpener.");
            }

            currentAngle += delta;

            if (currentAngle >= openAngle)
                isOpening = false;
        }
    }

    public void OpenHatch()
    {
        if (hingePoint == null)
        {
            Debug.LogError("HatchOpener requires a hingePoint to rotate around.");
            return;
        }

        isOpening = true;
    }
}
