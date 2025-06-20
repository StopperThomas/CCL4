using UnityEngine;

public class Sugar : MonoBehaviour
{
    public Transform targetPosition;
    public Transform gearToReveal;
    public Vector3 gearMoveOffset = new Vector3(0f, 0.05f, 0f);
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

                if (gearToReveal != null)
                {
                    RevealGear();
                }

                Destroy(gameObject, 0.1f);
            }
        }
    }

    public void OnInteract()
    {
        isMoving = true;
    }

    private void RevealGear()
    {
        gearToReveal.localPosition += gearMoveOffset;
        Debug.Log("Gear revealed!");
    }
}