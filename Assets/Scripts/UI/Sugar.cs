using UnityEngine;

public class Sugar : MonoBehaviour
{
    public Transform targetPosition;         // Where the cube flies to
    public HatchOpener hatchToOpen;          // Reference to the hatch script
    public float flySpeed = 2f;

    private bool isMoving = false;

    void Update()
    {
        if (isMoving && targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                isMoving = false;

                // Open the hatch
                if (hatchToOpen != null)
                    hatchToOpen.OpenHatch();

                // Despawn this object after a short delay (optional fade/anim time)
                Destroy(gameObject, 0.1f);
            }
        }
    }

    public void OnInteract()
    {
        isMoving = true;
    }
}