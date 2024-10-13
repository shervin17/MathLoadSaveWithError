using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadiingProfiles : MonoBehaviour
{
    [SerializeField] private SaveSlotButton profileButton;
    [SerializeField] private GameObject saveslots;
    [SerializeField] GameObject BackBtn;
    private Dictionary<string, GameData> existingProfiles;
    private List<SaveSlotButton> saveSlotsButtonList;

    void Awake()
    {
        existingProfiles = new Dictionary<string, GameData> ();
        saveSlotsButtonList = new List<SaveSlotButton>();
    }
    void Start()
    {
        loadProfile();
    }

    public void loadProfile()
    {
        string persistentPath = Application.persistentDataPath;
        Debug.Log(persistentPath);
        IEnumerable<DirectoryInfo> directoryInfos = new DirectoryInfo(persistentPath).EnumerateDirectories();

        float buttonSpacing = 120f; // Adjust this value for spacing between save slot buttons
        Vector3 currentPosition = new Vector3(); // Starting local position

        foreach (DirectoryInfo dirInfo in directoryInfos)
        {
            string fullpath = Path.Combine(persistentPath, dirInfo.Name, "user_data");
            if (!File.Exists(fullpath))
                continue;

            SaveSlotButton instance = Instantiate(profileButton, saveslots.transform);
            instance.transform.localPosition = currentPosition;
            instance.GetComponentInChildren<TextMeshProUGUI>().text = dirInfo.Name;
            currentPosition.y -= buttonSpacing;
            string profileKey = dirInfo.Name;
            instance.SetupButton(() => LoadGameProfile(profileKey));
            GameData gamedata = DataPersistenceManager.instance.fileDataHandler.LoadGameData(profileKey);
            if (gamedata != null)
            {
                existingProfiles.Add(profileKey, gamedata);
                saveSlotsButtonList.Add(instance);
            }
        }
        
        if(existingProfiles.Count > 0)
        {
            int target_index= existingProfiles.Count - 1;

            SaveSlotButton targetBtn = saveSlotsButtonList[target_index];

            Vector3 lastButtonPosition = targetBtn.transform.localPosition;

            Debug.Log($"Last button position: {lastButtonPosition.x}, {lastButtonPosition.y}");

            // Set the position of the Back button to be below the last button
            Vector3 backButtonPosition = lastButtonPosition;
            backButtonPosition.y -= buttonSpacing; // Move down by the same spacing used for the buttons
            BackBtn.transform.localPosition = backButtonPosition; // Set the new position
        }

    }

    public void LoadGameProfile(string profileID) {

        if (existingProfiles.TryGetValue(profileID, out GameData gamedata))
        {
            DataPersistenceManager.instance.SetCurrent_profileId(profileID);
            DataPersistenceManager.instance.SetGameData(gamedata);
            SceneManager.LoadSceneAsync("DashBoard");
        }
        else
        {
            Debug.LogWarning($"Profile with key '{profileID}' not found.");
            return;
        }
    }
}









