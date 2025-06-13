using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public float rayDistance = 5f;
    public InspectUIManager uiManager;
    public ObjectInspector inspector;
    public PlayerController playerController;

    private GameObject currentTarget;
    private bool isInspecting = false;

    private PlayerInputActions controls;

    private Screwdriver equippedScrewdriver;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.Player.Inspect.performed += ctx =>
        {
            if (isInspecting)
                ExitInspectMode();
            else
                TryInspect();
        };

        controls.Player.Cancel.performed += ctx =>
        {
            if (isInspecting)
                ExitInspectMode();
        };

        controls.Player.Click.performed += ctx =>
        {
            if (!isInspecting)
                TryClick();
        };
    }

    void Start()
    {
        // Restore equipped screwdriver from saved state
        if (GameStateManager.Instance != null)
        {
            string savedID = GameStateManager.Instance.GetEquippedScrewdriver();
            if (!string.IsNullOrEmpty(savedID))
            {
                Screwdriver found = FindScrewdriverByID(savedID);
                if (found != null)
                {
                    equippedScrewdriver = found;
                    Debug.Log("Equipped screwdriver restored: " + savedID);
                }
            }
        }
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (isInspecting) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Interactable") || hitObject.CompareTag("SceneChanger"))
            {
                if (hitObject != currentTarget)
                {
                    ClearHighlight();
                    currentTarget = hitObject;
                    EnableOutline(currentTarget, true);
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void TryInspect()
    {
        if (currentTarget == null || isInspecting) return;

        if (currentTarget.CompareTag("SceneChanger"))
        {
            SceneTransitionObject transition = currentTarget.GetComponent<SceneTransitionObject>();
            if (transition != null)
            {
                transition.LoadScene();
                return;
            }
        }

        if (currentTarget.CompareTag("Interactable"))
        {
            isInspecting = true;
            inspector.StartInspection(currentTarget);

            if (playerController != null) playerController.enabled = false;
        }

        Screw screw = currentTarget.GetComponent<Screw>();
        if (screw != null)
        {
            screw.TryUnscrew(equippedScrewdriver);
            return;
        }
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        inspector.EndInspection();

        if (playerController != null)
            playerController.enabled = true;
    }

    void ClearHighlight()
    {
        if (currentTarget != null)
        {
            EnableOutline(currentTarget, false);
            currentTarget = null;
        }
    }

    void EnableOutline(GameObject obj, bool enable)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable;
        }
    }

    void TryClick()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            Screw screw = hitObject.GetComponent<Screw>();
            if (screw != null)
            {
                screw.TryUnscrew(equippedScrewdriver);
                return;
            }

            Screwdriver screwdriver = hitObject.GetComponent<Screwdriver>();
            if (screwdriver != null)
            {
                SetEquippedScrewdriver(screwdriver);
                Debug.Log("Equipped screwdriver: " + screwdriver.name);
                return;
            }
        }
    }

    public void SetEquippedScrewdriver(Screwdriver screwdriver)
    {
        equippedScrewdriver = screwdriver;

        if (GameStateManager.Instance != null && screwdriver != null)
        {
            GameStateManager.Instance.SetEquippedScrewdriver(screwdriver.screwdriverID);
        }
    }

    // Helper method to find a screwdriver in the scene by its ID
    Screwdriver FindScrewdriverByID(string id)
    {
        Screwdriver[] all = FindObjectsOfType<Screwdriver>();
        foreach (var sd in all)
        {
            if (sd.screwdriverID == id)
                return sd;
        }
        return null;
    }
}
