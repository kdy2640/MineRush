using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    [SerializeField] private TMP_Text messageText;

    [SerializeField] private string message = "MINING START!";

    [SerializeField] private float panelOpenDuration = 0.45f;

    [SerializeField] private float panelCloseDuration = 0.45f;

    [SerializeField] private float textOpenDuration = 0.35f;
    [SerializeField] private float textCloseDuration = 0.35f;
    [SerializeField] private float waveDuration = 1.2f;

    public UnityEvent onFinished;

    private bool isPlaying;

    private void Awake()
    {
        if (panel == null)
        {
            Debug.LogError("[StartUI] Panel�� ������� �ʾҽ��ϴ�.");
        }

        if (messageText == null)
        {
            Debug.LogError("[StartUI] MessageText�� ������� �ʾҽ��ϴ�.");
        }
    }

    [ContextMenu("Debug/Play StartUI")]
    public void Play()
    {
        if (isPlaying) return;

        StartCoroutine(PlayRoutine());
    }

    public IEnumerator PlayRoutine()
    {
        isPlaying = true;

        panel.localScale = new Vector3(1f, 0f, 1f);

        messageText.rectTransform.localScale = Vector3.zero;

        messageText.text = message;

        Sequence seq = DOTween.Sequence();

        seq.Append(panel.
            DOScaleY(1f, panelOpenDuration)
                .SetEase(Ease.OutBack));

        seq.Join( messageText.rectTransform.
            DOScale(1f, textOpenDuration).SetEase(Ease.OutBack));

        yield return seq.WaitForCompletion();

        messageText.rectTransform.DOShakePosition(waveDuration,
            new Vector3(6f, 3f, 0f),
            20,
            90,
            false,
            true);

        messageText.rectTransform.DOShakeRotation(waveDuration,
            2f,
            20,
            90,
            false);

        yield return new WaitForSeconds(waveDuration);

        seq = DOTween.Sequence();

        seq.Append( messageText.rectTransform.DOScale(0f, textCloseDuration));

        seq.Join( panel.DOScaleY(0f, panelCloseDuration).SetEase(Ease.InBack));

        yield return seq.WaitForCompletion();

        isPlaying = false;

        gameObject.SetActive(false);

        onFinished?.Invoke();
    }
}