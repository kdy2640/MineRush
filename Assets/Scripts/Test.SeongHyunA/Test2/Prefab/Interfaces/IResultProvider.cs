using System.Collections.Generic;

public interface IResultProvider
{
    Dictionary<string, int> GetSessionData();

    int GetSessionXP();

    int GetTotalXP();

    int GetTotalOre(string oreName);
}