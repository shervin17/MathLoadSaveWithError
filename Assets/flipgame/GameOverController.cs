using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Reference to the Game Over Panel
    [SerializeField] private Button retryButton;       // Reference to the Retry Button
    [SerializeField] private Button mainMenuButton;    // Reference to the Main Menu Button

    private void Start()
    {
        // Ensure the Game Over panel is inactive at the start
        gameOverPanel.SetActive(false);

        // Add listeners to the buttons
        retryButton.onClick.AddListener(ReloadCurrentScene);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    public void ShowGameOverScreen()
    {
        // Show the Game Over panel
        gameOverPanel.SetActive(true);
    }

    private void ReloadCurrentScene()
    {
        // Reload the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void LoadMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("START"); // Replace with the actual name of your main menu scene
    }
}
