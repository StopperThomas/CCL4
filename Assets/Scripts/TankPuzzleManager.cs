using UnityEngine;

public class TankPuzzleManager : MonoBehaviour
{
    public static TankPuzzleManager Instance;

    public PointerController pointer;

    [Header("Acceptable Angle Range")]
    public float greenStartAngle = 45f;
    public float greenEndAngle = 60f;

    private bool puzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckIfInCorrectRange()
    {
        if (puzzleSolved) return;

        float angle = pointer.currentAngle;

        // Wrap around 0° - 360° if needed
        if (greenStartAngle > greenEndAngle)
        {
            if (angle >= greenStartAngle || angle <= greenEndAngle)
            {
                PuzzleSolved();
            }
        }
        else
        {
            if (angle >= greenStartAngle && angle <= greenEndAngle)
            {
                PuzzleSolved();
            }
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
