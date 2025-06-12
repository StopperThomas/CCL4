using UnityEngine;
using UnityEngine.UI;
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

    public void SetEquippedScrewdriver(Screwdriver screwdriver)
    {
        equippedScrewdriver = screwdriver;
    }


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

        // If it's a normal interactable object
        if (currentTarget.CompareTag("Interactable"))
        {
            isInspecting = true;
            inspector.StartInspection(currentTarget);

            // Disable player controller to stop movement & look
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

            // Check if it's a screw
            Screw screw = hitObject.GetComponent<Screw>();
            if (screw != null)
            {
                screw.TryUnscrew(equippedScrewdriver);
                return;
            }

            // Check if it's a screwdriver
            Screwdriver screwdriver = hitObject.GetComponent<Screwdriver>();
            if (screwdriver != null)
            {
                SetEquippedScrewdriver(screwdriver);
                Debug.Log("Equipped screwdriver: " + screwdriver.name);

                // Optionally, give player some feedback here, like UI update

                return;
            }
        }
    }
}
