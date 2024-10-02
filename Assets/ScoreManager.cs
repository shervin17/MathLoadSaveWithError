using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // Singleton instance

    public int score = 0;  // The player's score
    public Text scoreText; // Reference to the UI Text element that displays the score

    private void Awake()
    {
        // Singleton pattern to ensure there's only one ScoreManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this object persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event to handle UI updates when scenes change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Locate the new Score Text in the scene and update the reference
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
        UpdateScoreUI();  // Ensure the UI is updated with the current score
    }

    public void AddScore(int points)
    {
        score += points;  // Add points to the score
        UpdateScoreUI();  // Update the UI
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogWarning("Score Text not found or not assigned.");
        }
    }
}
