using System;
using System.Collections.Generic; 
using UnityEngine; 

public class TutorialManager : MonoBehaviour
{
    public enum TutorialType
    {
        BeforeGameLoop,
        UpgradeNode,
        UpgradePickaxe,
        UpgradeAutoMine,
        Length
    }
    private Dictionary<TutorialType, bool> TutorialProgressionMap = new();

    private void Awake()
    {
        Initialize();
    }
    // Enum 값에 해당하는 튜토리얼이 진행됐는지 확인하는 함수
    public bool GetTutorialProgressed(TutorialType type)
    {
        if(!TutorialProgressionMap.ContainsKey(type))
        {
            if(type >= TutorialType.Length)
            {
                throw new System.Exception("Invalid Tutorial Type");
            }
            TutorialProgressionMap.Add(type, false);
        }
        return TutorialProgressionMap[type];
    }
    // Enum 값에 해당하는 튜토리얼이 진행됐으면 외부에서 호출해주는 함수.
    public void ResolveTutorial(TutorialType type)
    { 
        if (!TutorialProgressionMap.ContainsKey(type))
        {
            Debug.LogError($"Invalid tutorial type: {type}");
            return;
        }
        TutorialProgressionMap[type] = true;
        GameManager.Instance.Save.SaveGame();
    }
    public void Initialize()
    {
        TutorialProgressionMap = new();
        for (int i = 0; i < (int)TutorialType.Length; i++)
        {
            TutorialProgressionMap.Add((TutorialType)i, false);
        } 
    }

    public List<TutorialSaveData> CreateTutorialSaveData()
    {
        List<TutorialSaveData> tutorialSaveDatas = new List<TutorialSaveData>();
        if (TutorialProgressionMap == null) Initialize();

        for (int i = 0; i < (int)TutorialType.Length; i++)
        {
            TutorialType nowType = (TutorialType)i;
            TutorialSaveData nowData = new TutorialSaveData(nowType.ToString(), TutorialProgressionMap[nowType]); 
            tutorialSaveDatas.Add(nowData);
        }
        return tutorialSaveDatas;
    }
    public void LoadTutorialSaveData(List<TutorialSaveData> tutorials)
    { 
        if (TutorialProgressionMap == null) Initialize();

        if (tutorials != null)
        {
            foreach (TutorialSaveData tutorialData in tutorials)
            {
                if (tutorialData == null)
                    continue;

                if (string.IsNullOrEmpty(tutorialData.id))
                    continue;

                bool result = Enum.TryParse<TutorialType>(tutorialData.id, out TutorialType type);

                if (!result || type < 0 || type >= TutorialType.Length)
                {
                    Debug.LogError($"Invalid TutorialType in SaveFile : {tutorialData.id}");
                    continue;
                } 

                TutorialProgressionMap[type] = tutorialData.flag; 
            }
        }
    }
    public void ResetSaveData()
    {
        Initialize();
    }

    [ContextMenu("LogNowTutorialValue")] 
    public void LogNowTutorial()
    {
        if (TutorialProgressionMap == null)
        {
            Debug.Log("TutorialProgressionMap is null");
            return;
        }

        Debug.Log("===== Tutorial Progression =====");

        for (int i = 0; i < (int)TutorialType.Length; i++)
        {
            TutorialType type = (TutorialType)i;

            if (TutorialProgressionMap.TryGetValue(type, out bool progressed))
            {
                Debug.Log($"{type} : {progressed}");
            }
            else
            {
                Debug.LogWarning($"{type} : Missing in TutorialProgressionMap");
            }
        } 
        Debug.Log("===============================");
    }
}
