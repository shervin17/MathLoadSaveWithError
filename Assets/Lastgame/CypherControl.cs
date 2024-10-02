using UnityEngine;
using UnityEngine.UI;

public class SignCipherGame : MonoBehaviour
{
    public Text[] signTexts;  // Assign these Text components in the Inspector to represent each letter
    public string correctPhrase = "CAT"; // The phrase we want to display
    public InputField userInput;
    public Text feedbackText;  // Text object to display feedback like a checkmark or message
    public Text scoreText; // Text object to display the score
    public Text timerText; // Text object to display the timer
    public GameObject completionPanel; // Reference to the completion panel
    public GameObject gameOverPanel; // Reference to the game over panel

    private int score = 0; // Variable to hold the current score
    public float timerDuration = 30f; // Duration of the timer in seconds
    private float timeRemaining;
    private bool gameComplete = false; // Flag to indicate if the game is complete

    // Start is called before the first frame update
    void Start()
    {
        DisplaySignCipher();
        feedbackText.enabled = false; // Ensure feedback text is hidden initially
        UpdateScoreUI(); // Update the score UI at the start
        timeRemaining = timerDuration; // Initialize the timer
        UpdateTimerUI(); // Update the timer display at the start
        completionPanel.SetActive(false); // Hide the completion panel at the start
        gameOverPanel.SetActive(false); // Hide the game over panel at the start
    }

    void Update()
    {
        // Update the timer if there is time remaining and the game is not complete
        if (timeRemaining > 0 && !gameComplete)
        {
            timeRemaining -= Time.deltaTime; // Decrease the remaining time
            UpdateTimerUI(); // Update the timer display
        }
        else if (timeRemaining <= 0 && !gameComplete)
        {
            TimeUp(); // Call the time up method when the timer reaches zero
        }
    }

    void DisplaySignCipher()
    {
        // Display each letter of the correct phrase
        for (int i = 0; i < correctPhrase.Length; i++)
        {
            // Set the text component to the corresponding letter
            signTexts[i].text = correctPhrase[i].ToString();
            signTexts[i].enabled = true; // Ensure the text is visible
        }
    }

    // Function to check if user input matches the correct answer
    public void CheckAnswer()
    {
        if (userInput.text.ToLower() == correctPhrase.ToLower()) // Compare in lowercase
        {
            feedbackText.text = "✔ Correct!";
            feedbackText.color = Color.green;
            feedbackText.enabled = true; // Ensure the text is visible

            // Increment score
            score += 10; // You can adjust the score value
            UpdateScoreUI(); // Update the score display

            // Show the completion panel and stop the timer
            ShowCompletionPanel();
            StopTimer();
        }
        else
        {
            feedbackText.text = "✘ Try Again";
            feedbackText.color = Color.red;
            feedbackText.enabled = true; // Ensure the text is visible
        }

        // Clear the input field after checking
        userInput.text = "";
    }

    // Function to stop the timer
    private void StopTimer()
    {
        gameComplete = true; // Set the game complete flag to true
        timeRemaining = 0; // Optionally set remaining time to 0 to ensure timer stops
    }

    // Function to update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString(); // Update the score display
        }
    }

    // Function to update the timer UI
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timeRemaining).ToString(); // Update the timer display
        }
    }

    private void TimeUp()
    {
        // Handle what happens when time runs out
        Debug.Log("Time's up! No more actions can be performed.");
        feedbackText.text = "✘ Time's Up!"; // Display time's up message
        feedbackText.color = Color.red;
        feedbackText.enabled = true; // Ensure the feedback text is visible

        // Show the game over panel
        ShowGameOverPanel();

        // Stop the timer
        StopTimer(); // Ensure the timer is stopped
    }

    private void ShowCompletionPanel()
    {
        // Show the completion panel and display the final score
        completionPanel.SetActive(true); // Show the completion panel
    }

    private void ShowGameOverPanel()
    {
        // Show the game over panel
        gameOverPanel.SetActive(true); // Show the game over panel
        // You can also set the text or score in the game over panel if desired
    }
}
