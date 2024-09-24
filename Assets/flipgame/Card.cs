using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer rend;

    [SerializeField] private Sprite faceSprite, backSprite;

    // Adjust these values to control the size
    [SerializeField] private Vector3 faceSpriteScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 backSpriteScale = new Vector3(1f, 1f, 1f);

    // The symbol this card corresponds to (e.g., A, B, C, etc.)
    [SerializeField] private string cardSymbol;

    private bool coroutineAllowed, facedUp;

    // Reference to the flipcontrol to manage game state
    private flipcontrol gameController;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = backSprite;
        transform.localScale = backSpriteScale; // Set the initial scale to the back sprite scale
        coroutineAllowed = true;
        facedUp = false;

        // Find the flipcontrol in the scene
        gameController = FindObjectOfType<flipcontrol>();
    }

    // Method to set the card symbol programmatically
    public void SetCardSymbol(string symbol)
    {
        cardSymbol = symbol;
    }

    private void OnMouseDown()
    {
        if (coroutineAllowed && !gameController.CardAlreadyFlipped())
        {
            StartCoroutine(RotateCard());
        }
        else
        {
            Debug.Log("Another card is already flipped!");
        }
    }

    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        if (!facedUp)
        {
            // Rotate the card to show the face
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = faceSprite; // Switch to face sprite
                    transform.localScale = faceSpriteScale; // Set scale to face sprite scale
                }
                yield return new WaitForSeconds(0.01f);
            }

            // Notify the flipcontrol that the card has been flipped
            gameController.CardFlipped(cardSymbol, this);
        }
        else
        {
            // Rotate the card back to show the back
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite; // Switch to back sprite
                    transform.localScale = backSpriteScale; // Set scale to back sprite scale
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        coroutineAllowed = true;
        facedUp = !facedUp;
    }

    // Add the ResetCard method here to reset the card after a wrong match
    public void ResetCard()
    {
        if (facedUp)
        {
            StartCoroutine(RotateCard()); // Flip the card back to the original side
        }
    }
}
