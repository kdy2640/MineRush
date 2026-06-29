using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public List<UpgradeSaveData> upgrades = new();
    public List<OreAmount> ores = new();
}

[Serializable]
public class UpgradeSaveData
{
    public string id;
    public int level;

    public UpgradeSaveData(string id, int level)
    {
        this.id = id;
        this.level = level;
    }
}