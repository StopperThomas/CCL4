using UnityEngine;
using System.Collections.Generic;

public class WireManager : MonoBehaviour
{
    [System.Serializable]
    public class WireConnection
    {
        public ScrewSocket socketA;
        public ScrewSocket socketB;
        public LineRenderer wireVisual;
        public Color wireColor;
    }

    public List<WireConnection> allConnections;

    public void EnableWiring()
    {
        foreach (var connection in allConnections)
        {
            if (connection.wireVisual != null)
            {
                connection.wireVisual.enabled = false;
                connection.wireVisual.startColor = connection.wireColor;
                connection.wireVisual.endColor = connection.wireColor;
            }
        }

        Debug.Log("Wiring enabled!");
    }

    public void Connect(ScrewSocket a, ScrewSocket b)
    {
        WireConnection found = allConnections.Find(c =>
            (c.socketA == a && c.socketB == b) || (c.socketA == b && c.socketB == a));

        if (found != null && found.wireVisual != null)
        {
            found.wireVisual.enabled = true;
            found.wireVisual.SetPosition(0, a.GetConnectionPoint().position);
            found.wireVisual.SetPosition(1, b.GetConnectionPoint().position);
            found.wireVisual.startColor = found.wireColor;
            found.wireVisual.endColor = found.wireColor;

            Debug.Log($"Connected wire between {a.name} and {b.name}");
        }
    }

    public bool AreAllConnectionsCorrect()
    {
        // Add your validation logic here if needed
        return true;
    }

    public void UpdateAllWires()
    {
        foreach (var connection in allConnections)
        {
            if (connection.wireVisual != null && connection.wireVisual.enabled)
            {
                connection.wireVisual.SetPosition(0, connection.socketA.GetConnectionPoint().position);
                connection.wireVisual.SetPosition(1, connection.socketB.GetConnectionPoint().position);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateAllWires(); // Ensure wires follow moving sockets if needed
    }


    public void TryAutoConnect(ScrewSocket updatedSocket)
    {
        foreach (var connection in allConnections)
        {
            bool matchesSocketA = connection.socketA == updatedSocket;
            bool matchesSocketB = connection.socketB == updatedSocket;

            // Proceed if the updated socket is part of the connection
            if (matchesSocketA || matchesSocketB)
            {
                ScrewSocket otherSocket = matchesSocketA ? connection.socketB : connection.socketA;

                if (updatedSocket.IsFilled && otherSocket.IsFilled)
                {
                    Connect(updatedSocket, otherSocket);
                }
            }
        }
    }

public void DisableAllWires()
{
    foreach (var connection in allConnections)
    {
        connection.wireVisual.enabled = false;
    }
}

}
