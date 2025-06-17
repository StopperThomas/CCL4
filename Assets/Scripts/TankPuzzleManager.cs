using UnityEngine;

public class TankPuzzleManager : MonoBehaviour
{
    public static TankPuzzleManager Instance;

    public PointerController pointer;

    
    public float greenStartAngle = 230f;
    public float greenEndAngle = 260f;

    public bool puzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckIfInCorrectRange()
    {
        if (puzzleSolved) return;

        float angle = pointer.currentAngle;
        Debug.Log($"Pointer angle (adjusted X): {angle}");

        if (greenStartAngle > greenEndAngle)
        {
            if (angle >= greenStartAngle || angle <= greenEndAngle)
                PuzzleSolved();
        }
        else
        {
            if (angle >= greenStartAngle && angle <= greenEndAngle)
                PuzzleSolved();
        }
    }

    public void PuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("Puzzle solved! Tank is full.");
        PromptManager.Instance?.ShowPrompt("The gauge points to green! The tank is filled.");
    }

    public void ResetPuzzle()
    {
        puzzleSolved = false;
        pointer.ResetPointer();
        PromptManager.Instance?.ShowPrompt("Puzzle reset.");
    }
}
