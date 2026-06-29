using UnityEngine;

public class SpawnOreWhenSecondSkill : SkillBase
{
    [field: SerializeField] public float timeIntervalPerLevel { get; private set; }

    private float lastActiveTime = float.MinValue;

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
        float currentTimeInterval = GetTimeInterval(level);

        string desc;

        if (level >= maxLevel)
        {
            desc = $"{currentTimeInterval:0.##}초마다";
        }
        else
        {
            float nextTimeInterval = GetTimeInterval(level + 1);
            desc = $"{currentTimeInterval:0.##}초마다 -> {nextTimeInterval:0.##}초마다";
        }

        return description.Replace("{계수}", desc);
    }

    public void HandleSecond(float time)
    {
        float currentTimeInterval = GetTimeInterval(level);

        if (Time.time - lastActiveTime > currentTimeInterval)
        {
            GameManager.Instance.GameLoop.SkillProxy.Execute(SkillType.SpawnOreWhenSecond);
            lastActiveTime = Time.time;
        }
    }

    private float GetTimeInterval(float targetLevel)
    {
        return timeIntervalPerLevel / (targetLevel + 1f);
    }
}