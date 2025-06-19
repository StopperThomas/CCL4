using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BookSelectable : MonoBehaviour
{
    public string bookID;
    public bool isCorrectBook;

    private bool isSelected = false;
    private List<Renderer> renderers = new();
    private List<Material> instanceMaterials = new();
    private List<Color> originalColors = new();
    private Coroutine pulseRoutine;

    private void Start()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var rend in renderers)
        {
            if (rend.material != null)
            {
                Material instanced = new Material(rend.material);
                Color original = instanced.HasProperty("_Color") ? instanced.color : Color.white;

                instanceMaterials.Add(instanced);
                originalColors.Add(original);
                rend.material = instanced;
            }
        }
    }


    public void ToggleSelection()
    {
        // Block interaction until engine puzzle is solved
        /*if (!PuzzleManager.Instance.IsEnginePuzzleSolved)
        {
            PromptManager.Instance?.ShowPrompt("These symbols look familiar... I've seen them elsewhere.");
            return;
        } */

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

    public bool IsSelected() => isSelected;

    private void UpdateVisual()
    {
        if (isSelected)
        {
            StartPulsing();
        }
        else
        {
            StopPulsing();
            RestoreOriginalColors();
        }
    }

    private void StartPulsing()
    {
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(PulseColor());
    }

    private void StopPulsing()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }
    }

    private IEnumerator PulseColor()
    {
        float time = 0f;
        Color pulseColor = Color.yellow;

        while (isSelected)
        {
            float t = Mathf.PingPong(time * 2f, 1f);
            for (int i = 0; i < instanceMaterials.Count; i++)
            {
                if (instanceMaterials[i].HasProperty("_Color"))
                {
                    Color baseColor = originalColors[i];
                    instanceMaterials[i].color = Color.Lerp(baseColor, pulseColor, t);
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        RestoreOriginalColors();
    }

    private void RestoreOriginalColors()
    {
        for (int i = 0; i < instanceMaterials.Count; i++)
        {
            if (instanceMaterials[i].HasProperty("_Color"))
            {
                instanceMaterials[i].color = originalColors[i];
            }
        }
    }
}
