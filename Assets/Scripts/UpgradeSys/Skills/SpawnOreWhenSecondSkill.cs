using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnOreWhenSecondSkill : SkillBase
{
    [field:SerializeField]public float chanceRatePerLevel { get; private set; }
    public override void Apply()
    {
        GameManager.Instance.GameLoop.SubscribeTick(HandleSecond); 
    }
    public override void Deactivate()
    {
        GameManager.Instance.GameLoop.UnSubscribeTick(HandleSecond); 
    }
    public override string GetFormattedDescription(int level, int maxLevel)
    {
        float currentChanceRate = level * chanceRatePerLevel;
        string desc;

        if (level >= maxLevel)
        {
            desc = $"+{currentChanceRate:0.##}%";
        }
        else
        {
            float nextChanceRate = (level + 1) * chanceRatePerLevel;
            desc = $"+{currentChanceRate:0.##}% -> +{nextChanceRate:0.##}%";
        }

        return description.Replace("{계수}", desc);
    }

    public void HandleSecond(float time)
    {
        float chanceRate = (level * chanceRatePerLevel);
        float randValue = Random.value;

        if (randValue <= chanceRate)
        { 
            GameManager.Instance.GameLoop.SkillProxy.Execute(SkillType.SpawnOreWhenSecond); 
        }
    }
}
