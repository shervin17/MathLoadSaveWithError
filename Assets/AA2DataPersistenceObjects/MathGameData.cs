using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class MathGameData 
{
    public int score;
    public int hints;
    public int unlocked_levels;
    public List<MathGameLevelData> levels;
    public MathGameData()
    { 
     score = 0;
     hints =3;
     unlocked_levels = 1 ;
     levels = new List<MathGameLevelData>();

        for (int i = 1; i <= 5; i++) 
        {
            if (i == 1)
            {
                levels.Add(new MathGameLevelData
                {
                    level = i,
                    isUnlocked = true
                });
            }
            else
            {
                levels.Add(new MathGameLevelData
                {
                    level = i,
                });
            }
        }
    }
    public override string ToString()
    {
        var levelDetails = string.Join("\n", levels.Select(level => level.ToString()));
        return $"Score: {score}, Hints: {hints}, Unlocked Levels: {unlocked_levels}\nLevels:\n{levelDetails}";
    }
}

