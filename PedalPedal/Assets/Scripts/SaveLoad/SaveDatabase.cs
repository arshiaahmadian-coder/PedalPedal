using System;
using System.Collections.Generic;

[Serializable]
public class Database
{
    public int coinAmount;
    public int highestScore;
    public int selectedSkinID;
    public int lastUnlockedLevelID;

    public List<int> ownedSkins;
    public List<int> levelData; // [2, 1, 3, 3, 3, 1, 2] -> [0]: level 1 passed with 2 stars

    public float masterSound;

    public Database()
    {
        coinAmount = 0;
        highestScore = 0;
        selectedSkinID = 0;
        lastUnlockedLevelID = 1;

        ownedSkins = new List<int>
        {
            selectedSkinID
        };
        
        levelData = new List<int> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        masterSound = 0.9f;
    }
}
