using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Cauldron : MonoBehaviour
{
    public List<string> correctPotions;
    public Transform liquidLayer;
    public float liquidRisePerPotion = 0.05f;
    public GameObject rewardCubePrefab;
    public Transform rewardSpawnPoint;

    private List<string> insertedPotions = new List<string>();
    private bool isChecking = false;
    private Vector3 originalLiquidPosition;

    private void Start()
    {
        if (liquidLayer != null)
        {
            originalLiquidPosition = liquidLayer.localPosition;
            Debug.Log("Original liquid position stored: " + originalLiquidPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChecking)
        {
            Debug.Log("Cauldron is currently checking combination. Ignoring further input.");
            return;
        }

        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
            Debug.Log("Potion entered cauldron: " + potion.potionID);

            if (insertedPotions.Count >= 3)
            {
                Debug.LogWarning("Already have 3 potions. Ignoring additional potion: " + potion.potionID);
                return;
            }

            insertedPotions.Add(potion.potionID);
            Debug.Log("Potion added. Current potions in cauldron: " + string.Join(", ", insertedPotions));

            potion.HidePotion();
            RaiseLiquid();

            if (insertedPotions.Count == 3)
            {
                Debug.Log("Three potions entered. Checking combination...");
                isChecking = true;
                CheckCombination();
            }
        }
        else
        {
            Debug.Log("Something entered cauldron but it wasn't a Potion: " + other.name);
        }
    }

    void RaiseLiquid()
    {
        if (liquidLayer == null)
        {
            Debug.LogError("Liquid layer is not assigned!");
            return;
        }

        Debug.Log("Raising liquid by " + liquidRisePerPotion);
        liquidLayer.localPosition += Vector3.up * liquidRisePerPotion;
        Debug.Log("New liquid position: " + liquidLayer.localPosition);
    }

    void CheckCombination()
    {
        var sortedCorrect = correctPotions.OrderBy(x => x).ToList();
        var sortedInput = insertedPotions.OrderBy(x => x).ToList();

        Debug.Log("Checking combination...");
        Debug.Log("Correct: " + string.Join(", ", sortedCorrect));
        Debug.Log("Input: " + string.Join(", ", sortedInput));

        if (sortedInput.SequenceEqual(sortedCorrect))
        {
            Debug.Log("Correct combination! Spawning reward.");
            Instantiate(rewardCubePrefab, rewardSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Incorrect combination. Resetting puzzle.");
            ResetPuzzle();
        }
    }

    void ResetPuzzle()
    {
        Debug.Log("Resetting puzzle...");

        foreach (Potion p in Resources.FindObjectsOfTypeAll<Potion>())
        {
            Debug.Log("Resetting potion: " + p.name);
            p.ResetPotion();
        }

        insertedPotions.Clear();
        isChecking = false;

        if (liquidLayer != null)
        {
            liquidLayer.localPosition = originalLiquidPosition;
            Debug.Log("Reset liquid level to original: " + originalLiquidPosition);
        }
    }
}