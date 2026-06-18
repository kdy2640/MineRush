using TMPro;
using UnityEngine;

public class StageTimerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameStateController controller;

    [SerializeField] private float stageTime = 60f;

    private float remainTime;
    private float lastStageTime;
    private bool isRunning;

    private void Start()
    {
        remainTime = stageTime;
        lastStageTime = stageTime;

        UpdateText();
    }
    private void Update()
    {
        if (!isRunning) return;

        if(!Mathf.Approximately(stageTime, lastStageTime))
        {
            float timeDifference = stageTime - lastStageTime;

            remainTime += timeDifference;

            lastStageTime = stageTime;
        }

        remainTime -= Time.deltaTime;

        if(remainTime <= 0)
        {
            remainTime = 0;
            isRunning = false;
            controller.ShowResult();
        }
        UpdateText();
    }
    public void StartTimer()
    {
        remainTime = stageTime;
        lastStageTime = stageTime; 
        isRunning = true;
    }
    private void UpdateText()
    {
        timerText.text = $"Remain : {remainTime:F0}";
    }

    
}
