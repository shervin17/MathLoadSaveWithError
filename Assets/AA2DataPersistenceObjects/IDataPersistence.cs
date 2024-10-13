using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    public void LoadGameData(GameData gamedata);
    public void SaveGameData(ref GameData gameData);

}
