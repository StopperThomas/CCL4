using UnityEngine;

public class CogwheelManager : MonoBehaviour
{
    public static CogwheelManager Instance;

    private Cogwheel heldCogwheel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PickUpCogwheel(Cogwheel cog)
    {
        heldCogwheel = cog;
    }

    public bool IsHoldingCogwheel() => heldCogwheel != null;
    public Cogwheel GetHeldCogwheel() => heldCogwheel;
    public void ClearHeldCogwheel() => heldCogwheel = null;
}
