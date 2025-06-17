using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public List<ScrewSocket> allScrewSockets;
    public WireManager wireManager;

    private void Awake()
    {
        Instance = this;
    }

    public void NotifyScrewPlaced(ScrewSocket socket)
    {
        // Automatically connect wires if both ends are filled
        wireManager.TryAutoConnect(socket);

        if (AllScrewsPlaced())
        {
            Debug.Log("All screws placed. Enabling wire connections.");
            wireManager.EnableWiring();
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

    public void ResetScrews()
{
    foreach (var socket in allScrewSockets)
    {
        socket.RemoveScrew();
    }

    Debug.Log("Screws reset and returned to inventory.");
    wireManager.DisableAllWires(); // Optional: reset wires visually
}

}
