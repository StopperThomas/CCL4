using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] private GameObject hingeObject;
    [SerializeField] private GameObject lightObject;

    [SerializeField] private Material redGlowMaterial;
    [SerializeField] private Material greenGlowMaterial;

    public static PuzzleDoor Instance;

    [SerializeField] private CogwheelSpot[] cogwheelSpots;
    [SerializeField] private GameObject doorObject;

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


    private bool doorOpened = false;

    private void OpenDoor()
    {
        if (doorOpened) return;
        doorOpened = true;

        Debug.Log("Opening door...");
        StartCoroutine(RotateDoor());

        // Change the material of the "Light" object
        MeshRenderer renderer = lightObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = greenGlowMaterial;
        }
    }


    private IEnumerator RotateDoor()
    {
        Quaternion startRot = hingeObject.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, -90f, 0); // Or -90f for opposite swing

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
