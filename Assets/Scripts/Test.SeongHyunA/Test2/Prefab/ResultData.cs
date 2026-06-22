using System.Collections.Generic;

[System.Serializable]
public class ResultData
{
    public int SessionXP;

    public int TotalXP;

    public List<ResultItemData> Items
        = new List<ResultItemData>();
}