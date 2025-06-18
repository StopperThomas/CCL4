using UnityEngine;
using System.Collections.Generic;

public class BookPuzzleManager : MonoBehaviour
{
    public static BookPuzzleManager Instance;

    public List<BookSelectable> allBooks;
    public Transform bookshelfToSlide;
    public Vector3 slideOffset;
    public float slideDuration = 1f;

    private List<BookSelectable> selectedBooks = new();
    private bool puzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanToggle(BookSelectable book)
    {
        if (puzzleSolved) return false;

        /*if (!PuzzleManager.Instance.IsEnginePuzzleSolved) // ← check from previous puzzle
        {
            PromptManager.Instance?.ShowPrompt("Hmm… have you seen symbols somewhere else?");
            return false;
        }*/

        return selectedBooks.Count < 4 || book.IsSelected(); 
    }
    

    public void HandleBookToggle(BookSelectable book, bool isSelected)
    {
        if (isSelected)
        {
            selectedBooks.Add(book);
        }
        else
        {
            selectedBooks.Remove(book);
        }

        if (selectedBooks.Count == 4)
            CheckSolution();
    }

    private void CheckSolution()
    {
        foreach (var book in selectedBooks)
        {
            if (!book.isCorrectBook)
            {
                PromptManager.Instance?.ShowPrompt("That doesn’t look quite right...");
                return;
            }
        }

        puzzleSolved = true;
        PromptManager.Instance?.ShowPrompt("You hear something move behind the shelf...");
        SlideBookshelf();
    }

    private void SlideBookshelf()
    {
        StartCoroutine(SlideCoroutine());
    }

    private System.Collections.IEnumerator SlideCoroutine()
    {
        Vector3 start = bookshelfToSlide.position;
        Vector3 target = start + slideOffset;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            bookshelfToSlide.position = Vector3.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bookshelfToSlide.position = target;
    }
}
