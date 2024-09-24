using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject I;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        I.SetActive(false);
    }
    public void Home()
    {
        SceneManager.LoadScene("Easy");
        Time.timeScale = 1;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        I.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale=1;
    }
}
