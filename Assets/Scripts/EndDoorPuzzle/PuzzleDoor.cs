using UnityEngine;
using System.Collections;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] private GameObject hingeObject;
    [SerializeField] private GameObject lightObject;

    [SerializeField] private Material redGlowMaterial;
    [SerializeField] private Material greenGlowMaterial;

    [SerializeField] private CogwheelSpot[] cogwheelSpots;
    [SerializeField] private GameObject doorObject;

    [SerializeField] private GameObject objectToUnhide;

    [Header("Wwise & Timing")]
    [SerializeField] private AK.Wwise.Event doorOpenSound;
    [SerializeField] private float doorOpenDuration = 2f;

    public static PuzzleDoor Instance;

    private bool doorOpened = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckPuzzleCompletion()
    {
        foreach (var spot in cogwheelSpots)
        {
            if (!spot.HasCorrectCogwheel())
            {
                Debug.Log("Puzzle not complete yet.");
                return;
            }
        }

        Debug.Log("All cogwheels placed correctly! Door opening...");
        OpenDoor();
    }

    private void OpenDoor()
    {
        if (doorOpened) return;
        doorOpened = true;

        Debug.Log("Opening door...");
        StartCoroutine(RotateDoor());

        // Change light material to green
        MeshRenderer renderer = lightObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = greenGlowMaterial;
        }

        // Unhide the object (if assigned)
        if (objectToUnhide != null)
        {
            objectToUnhide.SetActive(true);
        }
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRot = hingeObject.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, -90f, 0);

        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            hingeObject.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hingeObject.transform.rotation = targetRot;
    }
}