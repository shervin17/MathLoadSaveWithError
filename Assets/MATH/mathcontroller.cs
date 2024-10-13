using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mathcontroller : MonoBehaviour , IDataPersistence
{
    [System.Serializable]
    public class ButtonData
    {
        public Button button;   // The button itself
        public int value;       // The value the button holds
    }

    public int current_level;
    public ButtonData[] answerButtons;  // Array of buttons with values
    public Text questionMarkText;       // Text component for displaying the correct answer (the "?" box)
    public Text scoreText;              // Text component for displaying the score
    public Text timerText;              // Text component for displaying the timer
    public GameObject tryAgainPopup;    // Reference to the popup panel
    public float popupDuration = 2f;    // How long the popup should show
    public int correctAnswerIndex;      // Index of the correct answer
    public float timeLimit = 20f;       // Time limit for the level
    private float timer;                // Timer variable
    private int score;                  // Score variable
    private bool isCorrectAnswered;     // Flag to track if the correct answer was clicked
    private int hints;
    private bool isScored;
    private float bestTime;
    private int unlocked_levels;
    private bool unlock_next_level;
    private float time_consumed;
    [SerializeField] private GameObject completionPanel;

    void Start()
    {
        timer = timeLimit;  // Initialize timer
        score = 0;          // Initialize score
        isCorrectAnswered = false;  // Reset correct answer flag
        UpdateScore();      // Update score UI initially
        UpdateTimer();      // Update timer UI initially
        questionMarkText.text = "?"; // Set initial text for the question mark
        tryAgainPopup.SetActive(false); // Hide the popup at the start

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

    }
    private void ProceedToNextLevel()
    {
   if(completionPanel != null) 
            completionPanel.SetActive(true);
    }

    // Called when a button is clicked
    void OnAnswerClick(int index)
    {
        if (timer <= 0 || isCorrectAnswered) return;  // If time's up or already answered, do nothing

        // Check if the clicked button is the correct one
        if (index == correctAnswerIndex)
        {
            questionMarkText.text = answerButtons[index].value.ToString();  // Show the correct answer in the question mark box

            // Add to score
            time_consumed = timeLimit - timer;
            if (time_consumed < bestTime && bestTime > 0.0f)
                bestTime = time_consumed;
            if (current_level + 1 > unlocked_levels) {
                unlock_next_level = true;
                unlocked_levels = current_level + 1;
            }
            if (!isScored)
            {
                score += 10;
                isScored = true;
            }
            UpdateScore();                   // Update score display
            isCorrectAnswered = true;        // Mark that the correct answer has been given

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

    // Disable all buttons when time's up
    void DisableButtons()
    {
        foreach (ButtonData buttonData in answerButtons)
        {
            buttonData.button.interactable = false;
        }
    }

    public void LoadGameData(GameData gamedata)
    {
        var m  = gamedata.MathGameData;
        score= m.score;
        hints = m.hints;
        unlocked_levels = m.unlocked_levels;

        isScored= m.levels[current_level-1].isScored;
        bestTime = m.levels[current_level-1].bestTime;
    }

    public void SaveGameData(ref GameData gameData)
    {
       gameData.MathGameData.score = score;
       gameData.MathGameData.unlocked_levels = unlocked_levels;
       gameData.MathGameData.hints = hints;
       gameData.MathGameData.levels[current_level].isUnlocked= unlock_next_level;
       gameData.MathGameData.levels[current_level-1].bestTime = bestTime;
       gameData.MathGameData.levels[current_level-1].isScored = isScored;
    }
}
