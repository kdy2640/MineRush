using UnityEngine;

public class SkillBase : MonoBehaviour
{
    // 스킬 1개랑 1대1매칭 되는 스킬타입
    public enum SkillType
    {
        SpawnOreWhenMined
    } 
    [field: SerializeField]public string id { get; protected set; }
    [field:SerializeField]public float level { get; protected set; }
    // 여기의 레벨은 업그레이드 매니저에서 참조하거나 그런 건아님.
    // 업그레이드에선 data안에 실제 레벨이 있고,
    // 여기 있는 레벨은 루프 씬을 갔을때 데이터 연동후 레벨을 여기다 집어넣어서 실제 로직에서 작동되기 위한 것.
    [field:SerializeField, TextArea(5, 5)]public string description { get; protected set; }
    public virtual void Apply(){}
    public virtual void Deactivate(){}
    public void SetLevel(float level)
    {
        this.level = level;
    }
    public virtual string GetFormattedDescription(int level, int maxLevel)
    {
        return "";
    }
}
