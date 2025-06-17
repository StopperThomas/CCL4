using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleDoor : MonoBehaviour
{
    public static PuzzleDoor Instance;

    [SerializeField] private CogwheelSpot[] cogwheelSpots;
    [SerializeField] private GameObject doorObject;
    [SerializeField] private Animator doorAnimator;

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
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRot = doorObject.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, 90f, 0);

        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            doorObject.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorObject.transform.rotation = targetRot;
    }
}
