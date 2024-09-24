using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamesetup : MonoBehaviour
{
    // Assuming you have references to all your Card objects in the scene
    [SerializeField] private Card[] cards;

    // List of symbols you want to assign to the cards
    [SerializeField] private string[] symbols;

    void Start()
    {
        // Check if the number of cards matches the number of symbols
        if (cards.Length != symbols.Length)
        {
            Debug.LogError("Number of cards and symbols do not match!");
            return;
        }

        // Assign each symbol to a card
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetCardSymbol(symbols[i]);
        }
    }
}