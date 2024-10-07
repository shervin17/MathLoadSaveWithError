using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiGameLevelManager : MonoBehaviour
{
    public Button[] buttons;       // Buttons representing levels in the UI
    public string gameName;        // Name of the current game (e.g., "Game1", "Game2", etc.)

    private int levelsUnlocked;    // Tracks how many levels are unlocked for the current game
    private const int totalLevels = 5;   // Total number of levels per game

    void Start()
    {
        // Load the number of unlocked levels for the current game from PlayerPrefs
        levelsUnlocked = PlayerPrefs.GetInt(gameName + "_levelsUnlocked", 1); // Default to 1 level unlocked

        // Disable all buttons first
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // Enable the buttons for the unlocked levels
        for (int i = 0; i < levelsUnlocked; i++)
        {
            buttons[i].interactable = true;  // Make levels interactable up to the unlocked level
        }
    }

    // This method loads the scene (level) by its build index
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);  // Load the level scene by its build index
    }

    // Call this method when a player completes a level to unlock the next level
    public void UnlockNextLevel()
    {
        // Only unlock the next level if it hasn't already been unlocked
        if (levelsUnlocked < totalLevels)
        {
            levelsUnlocked++;
            PlayerPrefs.SetInt(gameName + "_levelsUnlocked", levelsUnlocked);
            PlayerPrefs.Save();
        }
    }

    // Call this method to reset the player's progress for this game
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(gameName + "_levelsUnlocked");  // Delete the progress for the current game
        levelsUnlocked = 1;  // Reset to only the first level unlocked
        Start();  // Reinitialize the buttons
    }
}
