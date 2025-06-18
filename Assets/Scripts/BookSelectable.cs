using UnityEngine;

public class BookSelectable : MonoBehaviour
{
    public string bookID; 
    public bool isCorrectBook;

    private bool isSelected = false;
    private Renderer bookRenderer;

    private void Start()
    {
        bookRenderer = GetComponent<Renderer>();
    }

    public void ToggleSelection()
    {
        if (!BookPuzzleManager.Instance.CanToggle(this)) return;

        isSelected = !isSelected;
        UpdateVisual();

        BookPuzzleManager.Instance.HandleBookToggle(this, isSelected);
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (bookRenderer != null)
        {
            bookRenderer.material.color = isSelected ? Color.yellow : Color.white;
        }
    }

    public bool IsSelected() => isSelected;
}
