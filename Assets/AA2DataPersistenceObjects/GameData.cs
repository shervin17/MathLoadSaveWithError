using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public class GameData 
{
    [SerializeField]
    public MathGameData MathGameData;
    public GameData()
    {
        MathGameData = new MathGameData();
    }
    public override string ToString()
    {
        return $"GameData:\n{MathGameData}"; 
    }

}
