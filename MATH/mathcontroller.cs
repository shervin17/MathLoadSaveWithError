using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mathcontrollers : MonoBehaviour
{
    [System.Serializable]
    public class ButtonData
    {
        public Button button;   // The button itself
        public int value;       // The value the button holds
    }

    public ButtonData[] answerButtons;  // Array of buttons with values
    public Text questionMarkText;       // Text component for displaying the correct answer (the "?" box)
    public Text scoreText;              // Text component for displaying the score
    public Text timerText;              // Text component for displaying the timer
    public GameObject tryAgainPopup;    // Reference to the popup panel
    public GameObject gameOverPanel;    // Reference to the Game Over panel
    public GameObject completionPanel;  // Reference to the completion panel
    public float popupDuration = 2f;    // How long the popup should show
    public int correctAnswerIndex;      // Index of the correct answer
    public float timeLimit = 60f;       // Time limit for the level
    private float timer;                // Timer variable
    private int score;                  // Score variable
    private bool isCorrectAnswered;     // Flag to track if the correct answer was clicked

    void Start()
    {
        timer = timeLimit;  // Initialize timer
        score = 0;          // Initialize score
        isCorrectAnswered = false;  // Reset correct answer flag
        UpdateScore();      // Update score UI initially
        UpdateTimer();      // Update timer UI initially
        questionMarkText.text = "?"; // Set initial text for the question mark
        tryAgainPopup.SetActive(false); // Hide the popup at the start
        gameOverPanel.SetActive(false); // Hide the Game Over panel at the start

        // Assign each button a listener
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;  // Local variable to avoid closure issue
            answerButtons[i].button.onClick.AddListener(() => OnAnswerClick(index));
        }
    }

    void Update()
    {
        // Timer countdown if the correct answer has not been clicked yet
        if (timer > 0 && !isCorrectAnswered)
        {
            timer -= Time.deltaTime;  // Decrease timer
            UpdateTimer();            // Update timer text
        }

        // If the timer reaches 0 and the game isn't already over
        if (timer <= 0 && !isCorrectAnswered)
        {
            timer = 0;  // Make sure the timer doesn't go below 0
            GameOver(); // Call Game Over method
        }
    }

    private void ProceedToNextLevel()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(true); // Show the completion panel
        }
    }

    // Called when a button is clicked
    void OnAnswerClick(int index)
    {
        if (timer <= 0 || isCorrectAnswered) return;  // If time's up or already answered, do nothing

        // Check if the clicked button is the correct one
        if (index == correctAnswerIndex)
        {
            questionMarkText.text = answerButtons[index].value.ToString();  // Show the correct answer in the question mark box
            score += 10;                     // Add to score
            UpdateScore();                   // Update score display
            isCorrectAnswered = true;        // Mark that the correct answer has been given

            ProceedToNextLevel(); // Show completion panel

            DisableButtons(); // Disable buttons after the correct answer
        }
        else
        {
            // Show "Try Again!" popup
            StartCoroutine(ShowTryAgainPopup());
        }
    }

    // Coroutine to show the Try Again popup for a short time
    IEnumerator ShowTryAgainPopup()
    {
        tryAgainPopup.SetActive(true);  // Show the popup
        yield return new WaitForSeconds(popupDuration);  // Wait for the specified time
        tryAgainPopup.SetActive(false);  // Hide the popup
    }

    // Updates the score UI
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    // Updates the timer UI
    void UpdateTimer()
    {
        timerText.text = "Time: " + Mathf.Max(timer, 0).ToString("F2");
    }

    // Game Over method
    void GameOver()
    {
        Debug.Log("Time's up! Game Over.");

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Disable all answer buttons
        DisableButtons();
    }

    // Disable all buttons when time's up
    void DisableButtons()
    {
        foreach (ButtonData buttonData in answerButtons)
        {
            buttonData.button.interactable = false;
        }
    }
}
