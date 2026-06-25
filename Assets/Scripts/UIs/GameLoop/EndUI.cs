using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    [SerializeField] private TMP_Text messageText;

    [SerializeField] private string message = "GAME OVER";

    [SerializeField] private float panelOpenDuration = 0.22f;

    [SerializeField] private float panelCloseDuration = 0.22f;

    [SerializeField] private float textDuration = 0.15f;

    [SerializeField] private float waveDuration = 0.45f;

    private bool isPlaying;

    private void Awake()
    {
        if (panel == null)
        {
            Debug.LogError("[EndUI] Panel이 연결되지 않았습니다.");
        }

        if (messageText == null)
        {
            Debug.LogError("[EndUI] MessageText가 연결되지 않았습니다.");
        }
    }

    public void Play()
    {
        if (isPlaying) return;

        gameObject.SetActive(true);

        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        isPlaying = true;

        panel.localScale = new Vector3(1f, 0f, 1f);

        messageText.rectTransform.localScale = Vector3.zero;

        messageText.text = message;

        Sequence seq = DOTween.Sequence();

        seq.Append(panel.
            DOScaleY(1f, panelOpenDuration)
                .SetEase(Ease.OutBack));

        seq.Join(messageText.rectTransform
                .DOScale(1f, textDuration)
                .SetEase(Ease.OutBack));

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

        seq.Append(
            messageText.rectTransform
                .DOScale(0f, textDuration));

        seq.Join(
            panel.DOScaleY(0f, panelCloseDuration)
                .SetEase(Ease.InBack));

        yield return seq.WaitForCompletion();

        gameObject.SetActive(false);

        isPlaying = false;
    }
}