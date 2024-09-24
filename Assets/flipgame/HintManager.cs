using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HintManager : MonoBehaviour
{
    // List of alphabet card GameObjects
    public List<GameObject> alphabetCards;

    // List of correct cards to not remove
    public List<GameObject> correctCards;

    // Hint button (assigned in Inspector)
    public Button hintButton;

    // Number of hints available
    public int hintCount = 3;

    // Optional: A UI text to display remaining hints
    public Text hintCountText;

    private void Start()
    {
        // Assign the hint button's onClick event to the UseHint method
        hintButton.onClick.AddListener(UseHint);

        // Initialize hint count display, if you're using one
        if (hintCountText != null)
        {
            hintCountText.text = "Hints Left: " + hintCount;
        }
    }

    // Method to remove one incorrect alphabet card when the hint button is clicked
    public void UseHint()
    {
        if (hintCount > 0)
        {
            // Get all incorrect cards
            List<GameObject> incorrectCards = new List<GameObject>();

            // Populate the list with only incorrect cards
            foreach (GameObject card in alphabetCards)
            {
                if (!correctCards.Contains(card) && card.activeSelf) // Only incorrect and currently active cards
                {
                    incorrectCards.Add(card);
                }
            }

            // Randomly remove one incorrect card
            if (incorrectCards.Count > 0)
            {
                int randomIndex = Random.Range(0, incorrectCards.Count);
                incorrectCards[randomIndex].SetActive(false); // Deactivate the randomly selected incorrect card
            }

            // Decrease hint count after performing the hint action
            hintCount--;

            // Update hint count text, if any
            if (hintCountText != null)
            {
                hintCountText.text = "Hints Left: " + hintCount;
            }

            // Disable the hint button if no more hints are left
            if (hintCount <= 0)
            {
                hintButton.interactable = false;
            }
        }
    }
}
