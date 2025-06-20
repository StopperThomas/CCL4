using UnityEngine;

public class DeskDrawer : MonoBehaviour
{
    public bool isLocked = false;
    public KeyType requiredKey;
    public bool isOpen = false;

    [Header("Movement Settings")]
    public Vector3 slideDirection = Vector3.forward;
    public float slideDistance = 0.3f;
    public float slideDuration = 1f;

    [Header("Puzzle Auto-Open")]
    public bool openOnTankPuzzleSolved = false;

    [SerializeField] private AK.Wwise.Event drawerInteractSound;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isSliding = false;
    private bool hasAutoOpened = false;

    private void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + slideDirection.normalized * slideDistance;
    }

    private void Update()
    {
        if (openOnTankPuzzleSolved && !isOpen && !isSliding && !hasAutoOpened)
        {
            if (TankPuzzleManager.Instance != null && TankPuzzleManager.Instance.tankPuzzleSolved)
            {
                PromptManager.Instance?.ShowPrompt("You hear a drawer unlock and slide open...");
                SlideDrawer(true);
                hasAutoOpened = true;
            }
        }
    }

    public void TryOpen(Item equippedItem)
    {
        if (isSliding) return;

        if (isLocked)
        {
            if (equippedItem != null &&
                equippedItem.itemType == ItemType.Key &&
                equippedItem.keyType == requiredKey)
            {
                isLocked = false;
                PromptManager.Instance?.ShowPrompt("Unlocked the drawer.");
                InventoryManager.Instance.inventory.RemoveItem(equippedItem);
                SlideDrawer(true);
            }
            else
            {
                PromptManager.Instance?.ShowPrompt("It's locked.");
            }
        }
        else
        {
            SlideDrawer(!isOpen);
        }
    }

    private void SlideDrawer(bool open)
    {
        if (drawerInteractSound != null)
            drawerInteractSound.Post(gameObject);

        isOpen = open;
        StartCoroutine(SlideCoroutine(open ? openPosition : closedPosition));
    }

    private System.Collections.IEnumerator SlideCoroutine(Vector3 target)
    {
        isSliding = true;
        Vector3 start = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            transform.localPosition = Vector3.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = target;
        isSliding = false;
    }
}