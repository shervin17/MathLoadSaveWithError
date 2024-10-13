using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{
    [Header("Loading UI Elements")]
    public GameObject loadingScreen;           // Reference to the entire loading screen panel
    public Transform inputLoadingBar;          // Loading bar transform (must have an Image component)

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
        sceneToLoad = sceneName;            // Set the scene to be loaded
        loadingScreen.SetActive(true);      // Show the loading screen
        StartCoroutine(LoadSceneAsync());   // Start loading the scene
    }

    private IEnumerator LoadSceneAsync()
    {
        currentValue = 0;  // Reset loading progress

        // Fake loading bar progress (simulate loading process)
        while (currentValue < 100)
        {
            currentValue += speedValue * Time.deltaTime;  // Increase progress over time
            inputLoadingBar.GetComponent<Image>().fillAmount = currentValue / 100;  // Update the loading bar
            yield return null;  // Wait for the next frame
        }

        // Once loading is done, load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
