using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private void OnEnable()
    {
        if (XPSystem.Instance != null)
            XPSystem.Instance.OnLevelUp += LevelUp;
    }

    private void OnDisable()
    {
        if (XPSystem.Instance != null)
            XPSystem.Instance.OnLevelUp -= LevelUp;
    }

    private void LevelUp(int level)
    {
        Debug.Log($"LEVEL UP : {level}");
    }
}