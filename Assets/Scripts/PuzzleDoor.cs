using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] private GameObject hingeObject;
    [SerializeField] private GameObject lightObject;

    [SerializeField] private Material redGlowMaterial;
    [SerializeField] private Material greenGlowMaterial;

    [SerializeField] private CogwheelSpot[] cogwheelSpots;
    [SerializeField] private GameObject doorObject;

    [Header("Wwise & Timing")]
    [SerializeField] private AK.Wwise.Event doorOpenSound; // Add Wwise event in Inspector
    [SerializeField] private float doorOpenDuration = 2f;  // Duration to fully open the door

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

        if (doorOpenSound != null)
            doorOpenSound.Post(gameObject);

        Debug.Log("Opening door...");
        StartCoroutine(RotateDoor());

        MeshRenderer renderer = lightObject.GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.material = greenGlowMaterial;
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRot = hingeObject.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, -90f, 0);

        float elapsed = 0f;

        while (elapsed < doorOpenDuration)
        {
            hingeObject.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / doorOpenDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hingeObject.transform.rotation = targetRot;
    }
}