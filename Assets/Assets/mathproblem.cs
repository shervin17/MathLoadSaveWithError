using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class MathProblem : MonoBehaviour
{
    // Reference to the image components
    public Image image3;
    public Image image4;

    // Reference to the answer buttons and feedback components
    public Button[] answerButtons; // Array of buttons for answers
    public Button hintButton; // Hint button
    public TextMeshProUGUI feedbackText; // For showing feedback
    public Animator feedbackAnimator; // Animator for feedback animations
    public TextMeshProUGUI scoreText; // Display the player's score

    // Values for the images
    private int value1;
    private int value2;
    private int correctAnswer;
    private int score = 0; // Player score

    // Start is called before the first frame update
    void Start()
    {
        // Read values from the image names
        value1 = GetValueFromImageName(image3);
        value2 = GetValueFromImageName(image4);
        correctAnswer = value1 + value2;

        // Add listeners for the hint button
        hintButton.onClick.AddListener(ShowHint);

        // Setup the answer buttons
        SetupAnswerButtons();

        // Update score display
        UpdateScore();
    }

    // Method to extract the value from the image name
    int GetValueFromImageName(Image img)
    {
        // Assuming the image names are in the format "imageX" where X is the value
        string imageName = img.sprite.name;
        Match match = Regex.Match(imageName, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        else
        {
            Debug.LogError("Invalid image name format. Expected a number in the name.");
            return 0;
        }
    }

    // Method to setup answer buttons with random answers including the correct one
    void SetupAnswerButtons()
    {
        // Generate a list of possible answers
        int[] possibleAnswers = GenerateAnswers();

        // Assign answers to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = possibleAnswers[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer.ToString(); // Set button text
            answerButtons[i].onClick.RemoveAllListeners(); // Remove old listeners
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer)); // Add new listener with the answer
        }
    }

    // Method to generate possible answers including the correct one
    int[] GenerateAnswers()
    {
        int[] answers = new int[answerButtons.Length];
        int correctIndex = Random.Range(0, answerButtons.Length); // Randomly place the correct answer

        // Fill the array with random incorrect answers
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = (i == correctIndex) ? correctAnswer : Random.Range(correctAnswer - 10, correctAnswer + 10);
        }

        // Ensure there are no duplicates that match the correct answer
        while (System.Array.Exists(answers, element => element == correctAnswer && System.Array.IndexOf(answers, correctAnswer) != correctIndex))
        {
            for (int i = 0; i < answers.Length; i++)
            {
                if (i != correctIndex) answers[i] = Random.Range(correctAnswer - 10, correctAnswer + 10);
            }
        }

        return answers;
    }

    // Method to check the user's answer from button click
    void CheckAnswer(int selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            feedbackText.text = "Correct Answer!";
            feedbackAnimator.SetTrigger("Correct"); // Play correct animation
            score += 10; // Increase score
            UpdateScore();
            // Logic for loading the next level or showing the next problem
        }
        else
        {
            feedbackText.text = "Incorrect Answer. Try Again!";
            feedbackAnimator.SetTrigger("Incorrect"); // Play incorrect animation
        }
    }

    // Method to show a hint to the user
    void ShowHint()
    {
        feedbackText.text = $"Hint: {value1} + {value2} = ?";
        feedbackAnimator.SetTrigger("Hint"); // Play hint animation
    }

    // Method to update the score display
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
