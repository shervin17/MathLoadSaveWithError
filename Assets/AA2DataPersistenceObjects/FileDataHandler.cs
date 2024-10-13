using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class FileDataHandler 
{
    private string dirPath;
    private string filename;

    public FileDataHandler(string dirPath, string filename)
    {
        this.dirPath = dirPath;
        this.filename = filename;
    }
    public GameData LoadGameData(string profileId)
    {
        string fullpath = Path.Combine(dirPath, profileId, filename);
        Debug.Log($"FDH loading {profileId} {fullpath}");

        GameData gamedata = null;

        if (File.Exists(fullpath))
        {
            try
            {
                using (FileStream filestream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(filestream))
                    {
                        string fromJson = streamReader.ReadToEnd();
                        gamedata = JsonUtility.FromJson<GameData>(fromJson);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogError("something went wrong." + exception.Message);
            }

        }
        else
            Debug.Log("FDH File not found " + fullpath);

        return gamedata;
    }

    public void SaveGameData(GameData gamedata, string profileId)
    {

        string fullpath = Path.Combine(dirPath, profileId, filename);

        Debug.Log($"FDH Saving gamedata {gamedata} , for {profileId} at path {fullpath}");

        Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

        try
        {
            string json = JsonUtility.ToJson(gamedata);
            using (FileStream filestream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(filestream))
                {
                    streamWriter.WriteLine(json);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("FDH Unable to save" + ex.Message);
        }
    }

}
