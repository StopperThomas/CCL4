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

        return selectedBooks.Count < 4 || book.IsSelected();
    }

    public void HandleBookToggle(BookSelectable book, bool isSelected)
    {
        if (isSelected)
        {
            selectedBooks.Add(book);
            Debug.Log($" Book ID: {book.bookID}");
        }
        else
        {
            selectedBooks.Remove(book);
            Debug.Log($" Book deselected: {book.bookID}");
        }

        if (selectedBooks.Count == 4)
            CheckSolution();
    }

    private void CheckSolution()
    {
        Debug.Log("📘 Checking selected books:");
        foreach (var book in selectedBooks)
        {
            Debug.Log($"Selected Book ID: {book.bookID}, Correct: {book.isCorrectBook}");
        }

        foreach (var book in selectedBooks)
        {
            if (!book.isCorrectBook)
            {
                PromptManager.Instance?.ShowPrompt("I know this combination... let's try again.");

                foreach (var b in selectedBooks)
                {
                    b.Deselect();
                }

                selectedBooks.Clear();
                Debug.Log(" Incorrect combination. Resetting selections.");
                return;
            }
        }

        puzzleSolved = true;
        PromptManager.Instance?.ShowPrompt("You hear something move behind the shelf...");
        Debug.Log(" Puzzle solved. Sliding bookshelf.");
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
