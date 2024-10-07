using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{
    [Header("Loading UI Elements")]
    public GameObject loadingScreen;           // Reference to the entire loading screen panel
    public Image inputLoadingBar;              // Loading bar image (must have an Image component with fillAmount)
    public Text percentageText;                // Text to display loading percentage
    public GameObject levelCompletionPanel;    // Reference to the level completion panel

    [Header("Loading Settings")]
    [SerializeField] private float speedValue = 32f;  // Speed of the loading bar (adjustable)
    private float currentValue;                // Current value of the loading progress
    private string sceneToLoad;                // Name of the scene to be loaded

    void Start()
    {
        currentValue = 0;  // Initialize the loading progress
        loadingScreen.SetActive(false);  // Ensure loading screen is hidden initially
    }

    // Call this method when clicking the button to load a new scene
    public void ShowLoadingScreenAndLoadScene(string sceneName)
    {
        if (levelCompletionPanel != null)
        {
            levelCompletionPanel.SetActive(false);  // Ensure the level completion panel is hidden
        }

        sceneToLoad = sceneName;  // Set the scene to be loaded
        loadingScreen.SetActive(true);  // Show the loading screen
        StartCoroutine(LoadSceneAsync());  // Start loading the scene
    }

    private IEnumerator LoadSceneAsync()
    {
        currentValue = 0;  // Reset loading progress

        // Simulate loading bar progress
        while (currentValue < 100)
        {
            currentValue += speedValue * Time.deltaTime;  // Increase progress over time
            inputLoadingBar.fillAmount = currentValue / 100;  // Update the loading bar's fill amount
            percentageText.text = Mathf.RoundToInt(currentValue) + "%";  // Update the percentage text
            yield return null;  // Wait for the next frame
        }

        // Once loading is done, load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
