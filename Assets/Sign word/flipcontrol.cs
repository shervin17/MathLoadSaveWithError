using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Flipcontrol : MonoBehaviour
{
    private string flippedCardSymbol;
    private Card flippedCard;

    [SerializeField] private Text scoreText;
    [SerializeField] private int scorePerMatch = 10;
    private int score = 0;

    private bool cardFlipped = false;

    [SerializeField] private int totalCards = 10;
    private int successfulMatches = 0;

    // List of scene names corresponding to levels
    public string[] levelNames;

    public GameOverController gameOverController; // Reference to your Game Over Controller

    // Timer-related variables
    [SerializeField] private Text timerText;
    [SerializeField] private float timeLimit = 60f; // 60 seconds for the timer
    private float currentTime;
    private bool timerRunning = true;

    [SerializeField] private GameObject levelCompletionPanel; // Reference to the panel
    [SerializeField] private Text finalScoreText;        // Reference to the score text
    [SerializeField] private Text timeLeftText;          // Reference to the current completion time text
    [SerializeField] private Text bestTimeText;          // Reference to the best completion time text
    [SerializeField] private Button nextLevelButton;     // Reference to the next level button
    [SerializeField] private GameObject gameOverPanel; // Reference to the Game Over Panel

    public loadflip loadingController;  // Reference to loadflip script

    void Start()
    {
        UpdateScoreText();
        StartTimer();
    }

    void Update()
    {
        // Check if the timer is running
        if (timerRunning)
        {
            // Update the current time
            currentTime -= Time.deltaTime;

            // Update the timer display
            UpdateTimerText();

            // Check if the time has run out
            if (currentTime <= 0)
            {
                currentTime = 0; // Ensure currentTime doesn't go negative
                timerRunning = false; // Stop the timer
                TimerEnded(); // Call the method to handle timer end
            }
        }
    }
    private void GameOver()
    {
        Debug.Log("Time's up! Game Over.");

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
      
        // Hide specific cards one by one
        GameObject cardA = GameObject.Find("CardA"); // Replace with the correct card names
        if (cardA != null)
        {
            cardA.SetActive(false);
        }
      
    }
    public void CardFlipped(string cardSymbol, Card card)
    {
        if (cardFlipped)
        {
            Debug.Log("Only one card can be flipped at a time!");
            return;
        }

        flippedCardSymbol = cardSymbol;
        flippedCard = card;
        cardFlipped = true;
    }

    public void OnAlphabetSelected(string selectedAlphabet)
    {
        if (!cardFlipped)
        {
            Debug.Log("You need to flip a card first!");
            return;
        }

        if (flippedCardSymbol != null && flippedCardSymbol == selectedAlphabet)
        {
            score += scorePerMatch;
            UpdateScoreText();
            Debug.Log("Correct match! Score: " + score);

            // Find the Button representing the alphabet letter
            Button alphabetButton = GameObject.Find(selectedAlphabet)?.GetComponent<Button>();

            StartCoroutine(RemoveCardAndLetter(alphabetButton));
        }
        else
        {
            Debug.Log("Wrong match!");
            ResetCard();
        }

        cardFlipped = false;
        flippedCardSymbol = null;
    }


    private IEnumerator RemoveCardAndLetter(Button alphabetButton)
    {
        yield return new WaitForSeconds(0.5f);

        // Destroy the flipped card
        if (flippedCard != null)
        {
            Destroy(flippedCard.gameObject);  // Remove the card from the game
            successfulMatches++;

            Debug.Log("Successful Matches: " + successfulMatches + " / " + totalCards);

            // Disable the corresponding alphabet button but keep it visible
            if (alphabetButton != null)
            {
                alphabetButton.interactable = false; // Disable interaction
                                                     // Optionally, change the button's appearance to indicate it has been matched
                                                     // For example, change color or make it semi-transparent:
                ColorBlock colorBlock = alphabetButton.colors;
                colorBlock.disabledColor = new Color(1, 1, 1, 0.5f); // Set transparency or another color
                alphabetButton.colors = colorBlock;
            }

            // Check if all cards are matched
            if (successfulMatches == totalCards)
            {
                Debug.Log("All cards matched! Proceeding to the next level...");
                ProceedToNextLevel();
            }
        }
    }



    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public bool CardAlreadyFlipped()
    {
        return cardFlipped;
    }

    private void ResetCard()
    {
        if (flippedCard != null)
        {
            flippedCard.ResetCard();
        }
    }

    public void LoadNextLevel()
    {
        // Get the highest unlocked level from PlayerPrefs
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Check if there's a next level to load
        if (unlockedLevel < levelNames.Length)  // Assuming levelNames is an array of level scene names
        {
            string nextLevelName = "FLIP " + (unlockedLevel + 1).ToString();  // Next level name in sequence

            // Use loadflip script to show the loading screen and load the next level
            if (loadingController != null)
            {
                loadingController.ShowLoadingScreenAndLoadScene(nextLevelName);  // Load the next level with the loading screen
            }
        }
    }

    private void ProceedToNextLevel()
    {
        // Deactivate game elements or stop gameplay if necessary
        timerRunning = false; // Stop the timer

        // Calculate the time left when the level was completed
        float timeLeft = currentTime; // Time left when the level was completed

        // Get the best time left for this level from PlayerPrefs
        string currentLevelName = SceneManager.GetActiveScene().name;
        float bestTimeLeft = PlayerPrefs.GetFloat(currentLevelName + "_BestTimeLeft", 0f);

        // Check if the current time left is better (more) than the best time left
        if (timeLeft > bestTimeLeft)
        {
            bestTimeLeft = timeLeft;
            PlayerPrefs.SetFloat(currentLevelName + "_BestTimeLeft", bestTimeLeft); // Save the new best time left
        }

     
        // Activate the level completion panel and display score, time left, and best time left
        if (levelCompletionPanel != null)
        {
            levelCompletionPanel.SetActive(true);

            // Stop all audio when the completion panel shows
            if (AudiomanagerFlip.Instance != null)
            {
                AudiomanagerFlip.Instance.StopAllAudio();
            }

            // Display the final score
            if (finalScoreText != null)
            {
                finalScoreText.text = "Final Score: " + score.ToString();
            }

            // Display the time left (format as minutes:seconds)
            if (timeLeftText != null)
            {
                int minutes = Mathf.FloorToInt(timeLeft / 60);
                int seconds = Mathf.FloorToInt(timeLeft % 60);
                timeLeftText.text = string.Format("Time Left: {0}:{1:00}", minutes, seconds);
            }

            // Display the best time left (format as minutes:seconds)
            if (bestTimeText != null)
            {
                int bestMinutes = Mathf.FloorToInt(bestTimeLeft / 60);
                int bestSeconds = Mathf.FloorToInt(bestTimeLeft % 60);
                bestTimeText.text = string.Format("Best Time Left: {0}:{1:00}", bestMinutes, bestSeconds);
            }

            // Optional: Configure the button to proceed to the next level or restart
            if (nextLevelButton != null)
            {
                nextLevelButton.onClick.RemoveAllListeners(); // Clear any previous listeners
                nextLevelButton.onClick.AddListener(LoadNextLevel); // Add listener for the next level
            }
        }
      
    }

    // Timer-related methods
    private void StartTimer()
    {
        currentTime = timeLimit;
        timerRunning = true;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);  // Get minutes
            int seconds = Mathf.FloorToInt(currentTime % 60);  // Get seconds

            // Format the time as "minutes:seconds", ensuring two digits for seconds
            timerText.text = string.Format("Time: {0}:{1:00}", minutes, seconds);
        }
    }

    private void TimerEnded()
    {
        Debug.Log("Time's up! Game Over.");
        // Call GameOver method to display the game over panel
        GameOver();

        
    }

}
