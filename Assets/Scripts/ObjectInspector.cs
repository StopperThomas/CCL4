using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public PlayerController playerController;

    private GameObject currentObject;
    private bool inspecting = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    void Update()
    {
        if (!inspecting) return;

        float rotX = Input.GetAxis("Mouse X") * 5f;
        float rotY = Input.GetAxis("Mouse Y") * 5f;

        currentObject.transform.Rotate(Vector3.up, -rotX, Space.World);
        currentObject.transform.Rotate(Vector3.right, rotY, Space.World);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndInspection();
        }
    }

    public void StartInspection(GameObject obj)
    {
        if (inspecting) return;

        inspecting = true;

        originalPosition = obj.transform.position;
        originalRotation = obj.transform.rotation;
        originalParent = obj.transform.parent;

        currentObject = obj;
        currentObject.GetComponent<Collider>().enabled = false;

        // Detach from any parent so it doesn't inherit unexpected transforms
        currentObject.transform.SetParent(null);

        // Position the object directly in front of the camera at a fixed distance
        Transform cam = Camera.main.transform;
        float inspectDistance = 2f; // Adjust distance as needed

        currentObject.transform.position = cam.position + cam.forward * inspectDistance;
        currentObject.transform.rotation = Quaternion.identity; // Or any rotation you want
    }

    public void EndInspection()
    {
        if (!inspecting) return;

        inspecting = false;

        currentObject.transform.SetParent(originalParent);
        currentObject.transform.position = originalPosition;
        currentObject.transform.rotation = originalRotation;
        currentObject.GetComponent<Collider>().enabled = true;

        currentObject = null;
        FindObjectOfType<InteractionManager>().enabled = true;
    }
}
