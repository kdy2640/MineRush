using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnLaserWhenMinedSkill : SkillBase
{
    [field:SerializeField]public float chanceRatePerLevel { get; private set; }
    public override void Apply()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.StoneDestroyed,HandleOreDestroyed); 
    }
    public override void Deactivate()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.StoneDestroyed, HandleOreDestroyed); 
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

    public void HandleOreDestroyed()
    {
        float chanceRate = (level * chanceRatePerLevel) / 100f;
        float randValue = Random.value;

        if (randValue <= chanceRate)
        { 
            GameManager.Instance.GameLoop.SkillProxy.Execute(SkillType.SpawnLaserWhenMined); 
        }
    }
}
