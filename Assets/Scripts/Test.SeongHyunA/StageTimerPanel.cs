using TMPro;
using UnityEngine;

public class UI_StageTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float stageTime = 60f;

    private float remainTime;

    private void Start()
    {
        remainTime = stageTime;
    }

    private void Update()
    {
        remainTime -= Time.deltaTime;

        if (remainTime < 0)
            remainTime = 0;

        timerText.text =
            $"{remainTime:F0}";
    }
}
