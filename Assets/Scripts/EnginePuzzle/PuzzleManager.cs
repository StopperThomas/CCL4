using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public List<ScrewSocket> allScrewSockets;
    public PromptManager promptManager;

    private bool puzzleSolved = false;
    public bool IsPuzzleSolved => puzzleSolved;

    private bool resetReady = false;
    public bool CanReset() => resetReady && !puzzleSolved;

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
            puzzleSolved = true;
            promptManager?.ShowPrompt("The connection is correct. Power flows through!");
            Debug.Log("Screw puzzle solved!");
        }
        else
        {
            resetReady = true; // ← enable reset
            promptManager?.ShowPrompt("Something's wrong. Press 'R' to try again.");
        }
    }

    public void ResetPuzzle()
    {
        foreach (var socket in allScrewSockets)
        {
            socket.RemoveScrew();
        }

        puzzleSolved = false;
        resetReady = false; // ← disable reset until next try
        promptManager?.ShowPrompt("Reset complete. Try again.");
    }
}
