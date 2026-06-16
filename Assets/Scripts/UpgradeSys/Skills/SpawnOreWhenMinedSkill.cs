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
        //context.OreManager.OnOreDestroyed += HandleOreDestroyed;
    }
    public override void Deactivate()
    {
        // context.OreManager.OnOreDestroyed -= HandleOreDestroyed;
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
            Debug.Log($"돌 생성!");
        }
    }
}
