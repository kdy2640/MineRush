using TMPro;
using UnityEngine;

public class UI_StageTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float stageTime = 60f;

    private float remainTime;
    private float lastStageTime;

    private void Start()
    {
        remainTime = stageTime;
        lastStageTime = stageTime;
    }

    private void Update()
    {
        if(!Mathf.Approximately(stageTime, lastStageTime))
        {
            float timeDifference = stageTime - lastStageTime;

            remainTime += timeDifference;

            lastStageTime = stageTime;
        }
        remainTime -= Time.deltaTime;

        if (remainTime < 0)
            remainTime = 0;

        timerText.text = $"Timer : {remainTime:F0}";
    }
}
