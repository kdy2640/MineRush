using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private readonly Dictionary<string, SkillBase> skills = new();

    public void RegisterSkill(SkillBase skillPrefab)
    {
        if (skillPrefab == null)
        {
            Debug.LogError("등록할 Skill prefab이 null입니다.");
            return;
        }
        if (string.IsNullOrWhiteSpace(skillPrefab.id))
        {
            Debug.LogError($"{skillPrefab.name}의 Skill id가 비어 있습니다. 프리팹의 SkillBase id를 입력해주세요.");
            return;
        }
        if (skills.ContainsKey(skillPrefab.id))
        {
            return;
        } // 이미 등록된 스킬. (왠만하면 중복으로 등록될 일은 없을거 같은데 임시로 방지해둠.)
        SkillBase skillInstance = Instantiate(skillPrefab, transform);
        skills.Add(skillInstance.id, skillInstance);
    }

    public void SetSkillLevel(string id, float level)
    {
        if (skills.TryGetValue(id, out SkillBase skill))
        {
            skill.SetLevel(level);
        }
    }

    public void ApplySkillsBeforeLoopSceneStart()
    {
        foreach (var skill in skills.Values)
        {
            skill.Apply();
        }
    }

    public void DeactivateSkillsBeforeLoopSceneExit()
    {
        foreach (var skill in skills.Values)
        {
            skill.Deactivate();
        }
    }
}
