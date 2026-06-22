using System;
using UnityEngine;

public class XPSystem : MonoBehaviour,IXPProvider
{
    public static XPSystem Instance;

    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    [SerializeField]
    private int requiredXP = 100;

    [SerializeField]
    private int level = 1;

    private int sessionXP;
    private int totalXP;
    private int levelXP;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(int amount)
    {
        sessionXP += amount;
        totalXP += amount;
        levelXP += amount;

        Debug.Log($"XP ADD : {amount}");
        Debug.Log($"CURRENT XP : {levelXP}");

        while (levelXP >= requiredXP)
        {
            levelXP -= requiredXP;

            level++;

            OnLevelUp?.Invoke(level);

            Debug.Log($"LEVEL UP : {level}");

            requiredXP += 50;
        }

        OnXPChanged?.Invoke( levelXP, requiredXP);
    }

    public int GetSessionXP()
    {
        return sessionXP;
    }

    public int GetTotalXP()
    {
        return totalXP;
    }

    public int GetLevelXP()
    {
        return levelXP;
    }

    public int GetRequiredXP()
    {
        return requiredXP;
    }

    public int GetLevel()
    {
        return level;
    }
    public int GetCurrentXP()
    {
        return levelXP;
    } 
    
    public void ResetSession()
    {
        sessionXP = 0;
    }

    public void ResetAll()
    {
        sessionXP = 0;
        totalXP = 0;
        levelXP = 0;

        level = 1;
        requiredXP = 100;

        OnXPChanged?.Invoke(
            levelXP,
            requiredXP);
    }
}