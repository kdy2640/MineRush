using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnOreWhenMinedSkill : SkillBase
{
    [field:SerializeField]public float baseChanceRate { get; private set; }
    [field:SerializeField]public float chanceRatePerLevel { get; private set; }
    public override void Apply()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.StoneDestroyed,HandleOreDestroyed); 
    }
    public override void Deactivate()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.StoneDestroyed, HandleOreDestroyed); 
    }
    public string GetFormattedDescription()
    {
        float chanceRate = baseChanceRate + (level * chanceRatePerLevel);
        return description
            .Replace("{확률}", chanceRate.ToString("0.##"));
    }

    public void HandleOreDestroyed()
    {
        float chanceRate = baseChanceRate + (level * chanceRatePerLevel);
        float randValue = Random.Range(0f, 100f);

        if (randValue <= chanceRate)
        { 
            GameManager.Instance.GameLoop.SkillProxy.Execute(SkillType.SpawnOreWhenMined); 
        }
    }
}
