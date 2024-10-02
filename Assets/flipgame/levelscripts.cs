using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelscripts : MonoBehaviour
{
    public void Pass()
    {
        int currentlevel = SceneManager.GetActiveScene().buildIndex;

        if (currentlevel >= PlayerPrefs.GetInt("levelsUnlocked"))
        {
            PlayerPrefs.SetInt("levelsUnlocked", currentlevel + 1);
        }
        Debug.Log("Level " + PlayerPrefs.GetInt("levelsUnlocked") + "UNLOCKED");
    }
    public void ResetLevels()
    {
        // Set levelsUnlocked to 1 (or another starting level)
        PlayerPrefs.SetInt("levelsUnlocked", 1);

        // Reset PlayerPrefs to save the changes
        PlayerPrefs.Save();


        Debug.Log("Levels have been reset.");
    }
}
