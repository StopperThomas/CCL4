using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public List<ScrewSocket> allScrewSockets;


    private bool enginePuzzleSolved = false;
    public bool IsEnginePuzzleSolved => enginePuzzleSolved;


    private void Awake()
    {
        Instance = this;
    }

    public void NotifyScrewPlaced(ScrewSocket socket)
    {
        if (AllScrewsPlaced())
        {
            CheckScrewPuzzle();
        }
    }

    private bool AllScrewsPlaced()
    {
        foreach (var socket in allScrewSockets)
        {
            if (!socket.IsFilled) return false;
        }
        return true;
    }

    private void CheckScrewPuzzle()
    {
        Debug.Log("Checking screw puzzle...");

        bool allCorrect = true;

        foreach (var socket in allScrewSockets)
        {
            if (socket.PlacedType != socket.requiredType)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            enginePuzzleSolved = true;
            Debug.Log("Screw puzzle solved!");
            PromptManager.Instance?.ShowPrompt("The connection is correct. Power flows through!");
            var interactionManager = FindObjectOfType<InteractionManager>();
            if (interactionManager != null)
                interactionManager.ForceExitInspectMode();
        }

    }
}
