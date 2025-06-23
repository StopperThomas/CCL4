using UnityEngine;
using System.Collections.Generic;

public class ScrewBox : MonoBehaviour
{
    [Header("Screws Holding the Lid")]
    [SerializeField] private List<Screw> _lidScrews;
    public List<Screw> lidScrews
    {
        get => _lidScrews;
        set => _lidScrews = value;
    }

    [Header("The Lid Object")]
    public GameObject lidObject;

    private bool lidRemoved = false;

    private void Awake()
    {
        // Ensure this script is only on valid objects
        if (lidObject == null && gameObject.name != "ScrewBox")
        {
            Debug.LogError($"ScrewBox is attached to '{gameObject.name}' but has no lidObject assigned.");
        }
    }

    private void Start()
    {
        // Auto-fill screws from children if not set
        if (lidScrews == null || lidScrews.Count == 0)
        {
            lidScrews = new List<Screw>(GetComponentsInChildren<Screw>());
        }
    }

    private void Update()
    {
        if (lidRemoved) return;

        if (AllScrewsUnscrewed())
        {
            RemoveLid();
        }
    }

    private bool AllScrewsUnscrewed()
    {
        if (lidScrews == null || lidScrews.Count == 0)
            return false;

        foreach (var screw in lidScrews)
        {
            if (screw == null || !screw.isUnscrewed)
                return false;
        }

        return true;
    }

    private void RemoveLid()
    {
        lidRemoved = true;

        if (lidObject != null)
        {
            Destroy(lidObject);
        }

        PromptManager.Instance?.ShowPrompt("The lid comes off!");
    }
}