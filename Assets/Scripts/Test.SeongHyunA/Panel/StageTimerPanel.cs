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
        timerText.text = $"Time : {remainTime:F0}";
    }
    //리스타트 버튼 누르면 호출
    public void ResetTimer()
    {
        remainTime = stageTime;
        lastStageTime = stageTime;

        UpdateText();
    }
}
