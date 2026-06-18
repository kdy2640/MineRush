using System;
using System.Collections.Generic;

// 사용 : 스킬 -> Execute
// 등록 : GameLoop Component -> Subscribe, UnSubscribe
// 접근 GameManager.Instance.GameLoop.EventProxy
public class SkillEventProxy
{
    private readonly Dictionary<SkillBase.SkillType, Action> actions = new();

    public void SubscribeAction(SkillBase.SkillType key, Action action)
    {
        if (actions.ContainsKey(key))
            actions[key] += action;
        else
            actions.Add(key, action);
    }

    public void UnSubscribeAction(SkillBase.SkillType key, Action action)
    {
        if (!actions.ContainsKey(key))
            return;

        actions[key] -= action;

        if (actions[key] == null)
            actions.Remove(key);
    }

    public void Execute(SkillBase.SkillType key)
    {
        if (actions.TryGetValue(key, out var action))
            action?.Invoke();
    }
}