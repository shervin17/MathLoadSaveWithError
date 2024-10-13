using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gamedata;
    public FileDataHandler fileDataHandler;
    private string current_profileID;
    List<IDataPersistence> persistenceObjects;

    public static DataPersistenceManager instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("instance is not null");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        Debug.Log($"Awake : instance is not null : {HasDPMinstance()} {instance}");
        DontDestroyOnLoad(this.gameObject);
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, "user_data");

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Debug.Log($"OnSceneLoaded : {HasDPMinstance()} {instance}");

        persistenceObjects = GetAllDataPersistenceObjects();

        if (current_profileID != null)
        {
            Debug.Log($" on load scene, profile id {current_profileID}");
            LoadGame();
        }

    }
    void OnSceneUnloaded(Scene scene)
    {
        if (gamedata != null)
            SaveGame();
    }


    public void NewGame()
    {

        this.gamedata = new GameData();

        Debug.Log($"New game : {instance} {gamedata}");
    }

    public void LoadGame()
    {
        Debug.Log($"DPM loading current profileID {current_profileID}");

        gamedata = fileDataHandler.LoadGameData(current_profileID);

        foreach (IDataPersistence persistenceObject in persistenceObjects)
        {
            Debug.Log($"persistent object : {persistenceObject}");
            persistenceObject.LoadGameData(gamedata);
        }

    }
    public void SaveGame()
    {
        Debug.Log($"DPM saving current profileID {current_profileID}");
        if (gamedata == null)
        {
            Debug.Log("gamedata is null");
            return;
        }
        foreach (IDataPersistence persistenceObject in persistenceObjects)
        {
            persistenceObject.SaveGameData(ref gamedata);
        }

        fileDataHandler.SaveGameData(gamedata, current_profileID);
    }

    public void SetCurrent_profileId(string profileID)
    {
        this.current_profileID = profileID;
    }
    public string GetProfileId()
    {
        return current_profileID;
    }

    public void SetGameData(GameData gamedata)
    {
        this.gamedata = gamedata;
    }


    void OnApplicationQuit()
    {
        SaveGame();
    }
    public List<IDataPersistence> GetAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> persistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        int count = persistenceObjects.Count();
        Debug.Log($"Found {count} IDataPersistence objects.");
        return new List<IDataPersistence>(persistenceObjects);
    }

    public bool HasDPMinstance()
    {
        return instance != null;
    }
}
