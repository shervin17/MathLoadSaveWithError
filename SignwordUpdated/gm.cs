using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LetterPuzzleGame : MonoBehaviour
{
    public Text[] blankBoxes;   // The empty boxes where letters are filled
    public Button[] letterButtons; // All the letter buttons
    public Button clearButton;  // Button to clear the filled letters
    public Button enterButton;  // Button to submit the answer
    public Button hintButton;   // Button to use hints
    public Text hintCounterText;  // Text to display how many hints are left
    public Text feedbackText;   // Text to show feedback (Correct/Incorrect or Empty Slot)
    public Text scoreText;      // Text to display the score
    public Text timerText;      // Text to display the timer
    public string correctWord = "USA";  // The correct answer
    public int maxHints = 3;    // Maximum number of hints allowed
    private int currentBlank = 0;  // Index to keep track of which blank to fill
    private int hintsLeft;      // Tracks the number of remaining hints
    private int score = 0;      // Variable to track the player's score
    private List<int> usedHints = new List<int>();  // Keeps track of which letters have already been removed

    private float timeLeft = 20f;  // 20-second timer
    private bool isTimerRunning = false;  // Flag to check if the timer is running
    private bool gameEnded = false;  // To stop the game when time runs out

    public GameObject completionPanel;   // Reference to the completion panel
    public GameObject gameOverPanel;     // Reference to the game over panel

    void Start()
    {
        hintsLeft = maxHints;
        UpdateHintCounter();
        UpdateScore();
        UpdateTimerText();

        if (completionPanel != null) completionPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        foreach (Button button in letterButtons)
        {
            button.onClick.AddListener(() => SelectLetter(button));
        }

        clearButton.onClick.AddListener(ClearBlanks);
        enterButton.onClick.AddListener(CheckAnswer);
        hintButton.onClick.AddListener(UseHint);
    }

    void Update()
    {
        if (isTimerRunning && !gameEnded)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
                GameOver();
            }
            UpdateTimerText();
        }
    }

    void SelectLetter(Button clickedButton)
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
        }

        if (currentBlank < blankBoxes.Length)
        {
            blankBoxes[currentBlank].text = clickedButton.GetComponentInChildren<Text>().text;
            clickedButton.interactable = false;
            currentBlank++;
        }
    }

    void ClearBlanks()
    {
        currentBlank = 0;
        foreach (Text blank in blankBoxes)
        {
            blank.text = "";
        }

        foreach (Button button in letterButtons)
        {
            button.interactable = true;
        }

        feedbackText.text = "";
        timeLeft = 20f;
        isTimerRunning = false;
        UpdateTimerText();
    }

    void CheckAnswer()
    {
        if (gameEnded)
        {
            return;
        }

        foreach (Text blank in blankBoxes)
        {
            if (string.IsNullOrEmpty(blank.text))
            {
                feedbackText.text = "Please fill all the slots!";
                feedbackText.color = Color.red;
                return;
            }
        }

        string playerWord = "";
        foreach (Text blank in blankBoxes)
        {
            playerWord += blank.text;
        }

        if (playerWord == correctWord)
        {
            feedbackText.text = "Correct! You've guessed the word!";
            feedbackText.color = Color.green;

            if (isTimerRunning)
            {
                AddScore(10);
                isTimerRunning = false;

                if (completionPanel != null)
                {
                    completionPanel.SetActive(true);
                }
            }
        }
        else
        {
            feedbackText.text = "Wrong Answer, Try Again!";
            feedbackText.color = Color.red;
        }
    }

    void UseHint()
    {
        if (gameEnded)
        {
            return;
        }

        if (hintsLeft > 0)
        {
            for (int i = 0; i < letterButtons.Length; i++)
            {
                Button button = letterButtons[i];
                string buttonLetter = button.GetComponentInChildren<Text>().text;

                if (!correctWord.Contains(buttonLetter) && button.interactable && !usedHints.Contains(i))
                {
                    button.interactable = false;
                    usedHints.Add(i);
                    hintsLeft--;
                    UpdateHintCounter();
                    return;
                }
            }
        }
        else
        {
            feedbackText.text = "No hints left!";
            feedbackText.color = Color.red;
        }
    }

    void UpdateHintCounter()
    {
        hintCounterText.text = "Hints Left: " + hintsLeft;
    }

    void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateTimerText()
    {
        timerText.text = "Time Left: " + Mathf.Ceil(timeLeft) + "s";
    }

    void GameOver()
    {
        isTimerRunning = false;
        gameEnded = true;
        feedbackText.text = "Time's up! Game over!";
        feedbackText.color = Color.red;

        foreach (Button button in letterButtons)
        {
            button.interactable = false;
        }

        enterButton.interactable = false;
        hintButton.interactable = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
