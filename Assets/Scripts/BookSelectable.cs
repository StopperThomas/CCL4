using UnityEngine;
using System.Collections;

public class BookSelectable : MonoBehaviour
{
    public string bookID;
    public bool isCorrectBook;

    private bool isSelected = false;
    private Renderer bookRenderer;
    private Material bookMaterial;
    private Coroutine pulseRoutine;

    private void Start()
    {
        bookRenderer = GetComponent<Renderer>();
        if (bookRenderer != null)
        {
            bookMaterial = Instantiate(bookRenderer.material); // make unique instance
            bookRenderer.material = bookMaterial;
        }
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
        if (bookMaterial == null) return;

        if (isSelected)
        {
            StartPulsing();
        }
        else
        {
            StopPulsing();
            bookMaterial.color = Color.white;
        }
    }

    public bool IsSelected() => isSelected;

    private void StartPulsing()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(PulseMaterialColor());
    }

    private void StopPulsing()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }
    }

    private IEnumerator PulseMaterialColor()
    {
        float time = 0f;
        Color baseColor = Color.white;
        Color pulseColor = Color.yellow;

        while (isSelected)
        {
            float t = Mathf.PingPong(time * 2f, 1f);
            bookMaterial.color = Color.Lerp(baseColor, pulseColor, t);
            time += Time.deltaTime;
            yield return null;
        }

        bookMaterial.color = baseColor;
    }
}
