using UnityEngine;
using UnityEngine.InputSystem;

public class CogwheelManager : MonoBehaviour
{
    public static CogwheelManager Instance;

    private Cogwheel heldCogwheel;
    private PlayerInputActions inputActions;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void TryInteract()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Cogwheel cog = hit.collider.GetComponent<Cogwheel>();
            if (cog != null && !IsHoldingCogwheel())
            {
                PickUpCogwheel(cog);
                cog.gameObject.SetActive(false);
                Debug.Log($"Picked up cogwheel of size {cog.size}");
                return;
            }

            CogwheelSpot spot = hit.collider.GetComponent<CogwheelSpot>();
            if (spot != null && IsHoldingCogwheel())
            {
                bool placed = spot.TryPlaceCogwheel(heldCogwheel);
                if (placed)
                {
                    ClearHeldCogwheel();
                }
                else
                {
                    Debug.Log("Wrong cogwheel size or spot already occupied.");
                }
            }

            // Try pick up key
            KeyPickup key = hit.collider.GetComponent<KeyPickup>();
            if (key != null)
            {
                key.PickUp();
                return;
            }

            // Try unlock lock
            BoxLock lockComponent = hit.collider.GetComponent<BoxLock>();
            if (lockComponent != null)
            {
                lockComponent.TryUnlock();
                return;
            }

            // Try open box
            Box box = hit.collider.GetComponent<Box>();
            if (box != null)
            {
                box.TryOpen();
                return;
            }
        }
    }

    public void PickUpCogwheel(Cogwheel cog)
    {
        heldCogwheel = cog;
    }

    public Cogwheel GetHeldCogwheel() => heldCogwheel;
    public bool IsHoldingCogwheel() => heldCogwheel != null;
    public void ClearHeldCogwheel() => heldCogwheel = null;
}
