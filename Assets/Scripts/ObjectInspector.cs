using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public PlayerController playerController;

    private GameObject currentObject;
    private bool inspecting = false;
    private bool isInventoryInspection = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    

   void Update()
{
    if (!inspecting || currentObject == null) return;

    float rotX = Input.GetAxis("Mouse X") * 5f;
    float rotY = Input.GetAxis("Mouse Y") * 5f;

    currentObject.transform.Rotate(Vector3.up, -rotX, Space.World);
    currentObject.transform.Rotate(Vector3.right, rotY, Space.World);

    if (Input.GetKeyDown(KeyCode.Escape))
    {
        EndInspection();
    }

    if (Input.GetKeyDown(KeyCode.E) && isInventoryInspection)
    {
    
        inspecting = !inspecting;
    }
}


    public void StartInspection(GameObject obj)
    {
        if (inspecting) return;

        inspecting = true;
        isInventoryInspection = false;

        originalPosition = obj.transform.position;
        originalRotation = obj.transform.rotation;
        originalParent = obj.transform.parent;

        currentObject = obj;
        currentObject.GetComponent<Collider>().enabled = false;

        currentObject.transform.SetParent(null);

        Transform cam = Camera.main.transform;
        float inspectDistance = 2f;
        currentObject.transform.position = cam.position + cam.forward * inspectDistance;
        currentObject.transform.rotation = Quaternion.identity;
    }
    public void StartInventoryInspection(GameObject obj)
    {
        if (currentObject != null)
            Destroy(currentObject);

        isInventoryInspection = true;
        inspecting = true;

        currentObject = obj;
    }


    public void EndInspection()
    {
        if (!inspecting && !isInventoryInspection) return;

        if (!isInventoryInspection)
        {
            
            currentObject.transform.SetParent(originalParent);
            currentObject.transform.position = originalPosition;
            currentObject.transform.rotation = originalRotation;
            currentObject.GetComponent<Collider>().enabled = true;

            FindObjectOfType<InteractionManager>().enabled = true;
        }
        else
        {
            
            if (currentObject != null)
            {
                Destroy(currentObject);
                currentObject = null;
            }
        }

        inspecting = false;
        isInventoryInspection = false;
        currentObject = null;
    }
}
