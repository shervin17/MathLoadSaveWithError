using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MathGameLevelData 
{
    public int level;
    public bool isScored;
    public float bestTime;
    public bool isUnlocked;

    public override string ToString()
    {
        return $"Level {level}: Unlocked = {isUnlocked}, isScored = {isScored}, Best Time = {bestTime:F2} seconds";
    }
}
