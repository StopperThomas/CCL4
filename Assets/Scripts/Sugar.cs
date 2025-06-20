using UnityEngine;

public class Sugar : MonoBehaviour
{
    public Transform targetPosition;
    public HatchOpener hatchToOpen;
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

                if (hatchToOpen != null)
                    hatchToOpen.OpenHatch();

                Destroy(gameObject, 0.1f);
            }
        }
    }

    public void OnInteract()
    {
        isMoving = true;
    }
}