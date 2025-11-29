using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int totalCredits;
    public int totalAbilityPoints;

    // Default constructor for a fresh save
    public GameData()
    {
        totalCredits = 0;
        totalAbilityPoints = 0;
    }
}