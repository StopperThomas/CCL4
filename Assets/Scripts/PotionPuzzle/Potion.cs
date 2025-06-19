using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour
{
    public string potionID;
    public Transform cauldronTarget;

    [SerializeField] private AK.Wwise.Event potionFlySound; // Assign in Inspector

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    private void Awake()
    {
        CacheComponents();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void CacheComponents()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void FlyToCauldron()
    {
        if (potionFlySound != null)
        {
            potionFlySound.Post(gameObject);
        }

        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {
        CacheComponents();

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        Vector3 start = transform.position;
        Vector3 end = cauldronTarget.position + Vector3.up * 2f;

        float t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime * 1.5f;
            yield return null;
        }

        rb.useGravity = true;
        GetComponent<Collider>().enabled = true;
    }

    public void ResetPotion()
    {
        CacheComponents();

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void HidePotion()
    {
        CacheComponents();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        gameObject.SetActive(false);
    }
}
