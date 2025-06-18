using UnityEngine;

public class TankPuzzleManager : MonoBehaviour
{
    public static TankPuzzleManager Instance;

    public PointerController pointer;

    
    public float greenStartAngle = 230f;
    public float greenEndAngle = 260f;

    public bool tankPuzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckIfInCorrectRange()
    {
        if (tankPuzzleSolved) return;

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
        tankPuzzleSolved = true;
        Debug.Log("Puzzle solved! Tank is full.");
        PromptManager.Instance?.ShowPrompt("The gauge points to green! The tank is filled. A Drawer somewhere opened.");

    }

    public void ResetPuzzle()
    {
        tankPuzzleSolved = false;
        pointer.ResetPointer();
        PromptManager.Instance?.ShowPrompt("Puzzle reset.");
    }
}
