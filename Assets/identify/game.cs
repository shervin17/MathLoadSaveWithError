using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene management
using System.Collections.Generic; // For List<T>

public class QuizManager : MonoBehaviour
{
    [SerializeField] private Button[] answerButtons;  // Array to hold answer buttons
    [SerializeField] private Button enterButton;      // Reference to the Enter button
    [SerializeField] private Button restartButton;    // Reference to the Restart button
    [SerializeField] private Button hintButton;       // Reference to the Hint button
    [SerializeField] private Image questionImage;     // Reference to the image at the top (e.g., Pepsi can)
    [SerializeField] private Text feedbackText;       // Text to display feedback
    [SerializeField] private Text scoreText;          // Text to display the score
    [SerializeField] private Text timerText;          // Text to display the timer
    [SerializeField] private Text hintCountText;      // Text to display the number of hints left
    [SerializeField] private string correctAnswer;    // The correct answer to be set in the Inspector
    [SerializeField] private string noAnswerMessage = "Please select an answer before clicking Enter."; // Message for no answer selected
    [SerializeField] private string timeUpMessage = "Time's up!"; // Message for time up

    private string selectedAnswer;
    private Button selectedButton; // To keep track of the selected button
    private int score = 0;  // Initialize score at 0
    private bool answeredCorrectly = false;  // To check if the correct answer was already given
    private float timeRemaining = 20f; // Timer set to 20 seconds
    private bool isGameActive = true; // To check if the game is active
    private int hintCount = 3; // Number of hints available
    private List<Button> disabledHintButtons = new List<Button>(); // To track buttons disabled by hints

    void Start()
    {
        // Add click listeners to answer buttons
        foreach (Button btn in answerButtons)
        {
            btn.onClick.AddListener(() => OnAnswerSelected(btn));
        }

        // Add click listener to the Enter button
        enterButton.onClick.AddListener(CheckAnswer);

        // Add click listener to the Restart button
        restartButton.onClick.AddListener(RestartQuiz);

        // Add click listener to the Hint button
        hintButton.onClick.AddListener(UseHint);

        // Initialize the score display
        UpdateScoreText();

        // Initialize the hint count display
        UpdateHintCountText();

        // Hide the restart button initially
        restartButton.gameObject.SetActive(false);

        // Set the initial timer text
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
    }

    void Update()
    {
        if (!isGameActive) return; // Skip updating if the game is not active

        // Update the timer
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            TimeUp(); // Call method to handle time up
        }

        // Update the timer text display
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
    }

    // This method is called when an answer button is clicked
    void OnAnswerSelected(Button clickedButton)
    {
        if (!isGameActive || answeredCorrectly || restartButton.gameObject.activeSelf) return; // Prevent selection if not active or already answered correctly or during retry

        // Start the timer when the user selects an answer
        if (timeRemaining == 20f) // Ensure timer starts only once
        {
            isGameActive = true; // Ensure the game is active
            timeRemaining = 20f; // Reset the timer
        }

        selectedAnswer = clickedButton.GetComponentInChildren<Text>().text;
        selectedButton = clickedButton; // Store the clicked button

        // Reset all buttons' color to default (black)
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.black; // Reset color
        }

        // Change color of the clicked button to green
        selectedButton.GetComponent<Image>().color = Color.green; // Highlight the clicked button
    }

    // This method is called when the Enter button is clicked
    void CheckAnswer()
    {
        if (!isGameActive) return; // Skip checking if the game is not active

        if (string.IsNullOrEmpty(selectedAnswer))
        {
            feedbackText.text = noAnswerMessage; // Show message if no answer is selected
            return; // Exit method to prevent further checking
        }

        if (answeredCorrectly) return;  // Prevent re-scoring if the correct answer has already been given

        if (selectedAnswer == correctAnswer)
        {
            feedbackText.text = "Correct!";

            // Only increment score if it hasn't been updated yet
            score += 10;
            UpdateScoreText();

            // Set answeredCorrectly to true to prevent further scoring
            answeredCorrectly = true;

            // Stop the timer
            isGameActive = false;

            // Disable all the wrong answer buttons
            DisableWrongAnswers();

            // Disable the hint button since the user got the correct answer
            hintButton.interactable = false;

            // No scene transition required for completion
        }
        else
        {
            feedbackText.text = "Wrong! Try again.";

            // Change the color of the wrong selected button to red
            if (selectedButton != null)
            {
                selectedButton.GetComponent<Image>().color = Color.red;
            }

            // Disable all answer buttons to prevent further selection
            SetAnswerButtonsInteractable(false);

            // Show the restart button so the user can retry
            restartButton.gameObject.SetActive(true);
        }
    }

    // Method to handle when time is up
    void TimeUp()
    {
        isGameActive = false; // Stop the game

        feedbackText.text = timeUpMessage;

        // Disable all answer buttons
        SetAnswerButtonsInteractable(false);

        // Disable the Enter button
        enterButton.interactable = false;

        // Optionally hide or disable the hint button
        hintButton.interactable = false;

        // No scene transition required for game over
    }

    // Update the score text display
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Update the hint count text display
    void UpdateHintCountText()
    {
        hintCountText.text = "Hints Left: " + hintCount;
    }

    // Disable all wrong answer buttons once the correct answer is chosen
    void DisableWrongAnswers()
    {
        foreach (Button btn in answerButtons)
        {
            if (btn.GetComponentInChildren<Text>().text != correctAnswer)
            {
                btn.interactable = false;  // Disable the wrong answer buttons
            }
        }
    }

    // Set interactable state for all answer buttons
    void SetAnswerButtonsInteractable(bool state)
    {
        foreach (Button btn in answerButtons)
        {
            btn.interactable = state;
        }
    }

    // Restart the quiz by resetting everything except the timer and hint states
    void RestartQuiz()
    {
        // Reset feedback text
        feedbackText.text = "";

        // Reset button colors and re-enable all buttons
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.black; // Reset color
            // Reapply disabled states from hints
            if (disabledHintButtons.Contains(btn))
            {
                btn.interactable = false;
            }
            else
            {
                btn.interactable = true;
            }
        }

        // Hide the restart button
        restartButton.gameObject.SetActive(false);

        // Reset the selected answer and answeredCorrectly flag
        selectedAnswer = "";
        answeredCorrectly = false;

        // Re-enable the hint button
        hintButton.interactable = true;
    }

    // Use hint by disabling one wrong answer button
    void UseHint()
    {
        if (!isGameActive || answeredCorrectly || hintCount <= 0) return; // Prevent hint use if the game is not active, answer is already correct, or no hints left

        // Get a list of wrong answer buttons
        var wrongAnswerButtons = new List<Button>();

        foreach (Button btn in answerButtons)
        {
            if (btn.GetComponentInChildren<Text>().text != correctAnswer && !disabledHintButtons.Contains(btn))
            {
                wrongAnswerButtons.Add(btn);  // Add wrong answer buttons to the list
            }
        }

        if (wrongAnswerButtons.Count > 0)
        {
            // Randomly select one wrong answer button to disable
            int randomIndex = Random.Range(0, wrongAnswerButtons.Count);
            Button buttonToDisable = wrongAnswerButtons[randomIndex];

            buttonToDisable.interactable = false;  // Disable the selected wrong answer button

            // Track the button that was disabled by the hint
            if (!disabledHintButtons.Contains(buttonToDisable))
            {
                disabledHintButtons.Add(buttonToDisable);
            }

            // Decrease the hint count and update the hint count text
            hintCount--;
            UpdateHintCountText();
        }

        // Disable the hint button if no hints are left
        if (hintCount <= 0)
        {
            hintButton.interactable = false;
        }
    }
}
