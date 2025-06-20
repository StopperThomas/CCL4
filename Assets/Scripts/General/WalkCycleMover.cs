using UnityEngine;

public class WalkCycleMover : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string walkStateName = "Walk";
    [SerializeField] private Vector3 walkOffset = new Vector3(0, 0, 3);
    [SerializeField] private float walkDuration = 2f;

    private Vector3 startPosition;
    private bool walkingForward = true;
    private bool isWalking = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName(walkStateName) && !isWalking)
        {
            isWalking = true;
            if (walkingForward)
            {
                StartCoroutine(WalkTo(startPosition + walkOffset));
            }
            else
            {
                StartCoroutine(WalkTo(startPosition));
            }
        }
        else if (!currentState.IsName(walkStateName) && isWalking)
        {
            isWalking = false;
        }
    }

    private System.Collections.IEnumerator WalkTo(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < walkDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / walkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        // Flip direction
        walkingForward = !walkingForward;

        // Rotate character 180 degrees on Y-axis
        transform.Rotate(0f, 180f, 0f);
    }
}