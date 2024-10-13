using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject saveSlots;
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject MainMenu;
    [SerializeField] TMP_InputField inputField;




    public void OnNewGameClicked()
    {
        setMenuOnOff();
    }
    public void setMenuOnOff()
    {
        if (MainMenu.activeSelf)
        {
            Panel.SetActive(true);
            MainMenu.SetActive(false);
        }
        else
        {
            Panel.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    public void setSaveSlotsOnOff()
    {
        if (saveSlots.activeSelf)
        {
            saveSlots.SetActive(false);
            MainMenu.SetActive(true);
        }
        else {
            saveSlots.SetActive(true);
            MainMenu.SetActive(false);
        }
    }
    public void createNewProfile()
    {
        string username = inputField.text;
        Debug.Log("created a new user " + username);
        /*setMenuOnOff();*/
        DataPersistenceManager dpm = DataPersistenceManager.instance;
        Debug.Log($"dpm is null: {dpm == null}");
        dpm.SetCurrent_profileId(username);
        dpm.NewGame();
        SceneManager.LoadSceneAsync("Dashboard");
    }
}
