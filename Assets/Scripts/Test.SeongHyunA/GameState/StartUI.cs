using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform startText;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private StageTimerPanel timer;
    [SerializeField] private GameStateController controller;

    private void Start()
    {
        PlayStartSequence();
    }

    private void PlayStartSequence()
    {
        controller.StartGame();

        startText.localScale = Vector3.zero;
        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            canvasGroup.DOFade(1f, 0.3f)
        );

        seq.Join(
            startText.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
        );

        seq.AppendInterval(1f);

        seq.Append(
            canvasGroup.DOFade(0f, 0.3f)
        );

        seq.Join(
            startText.DOScale(0f, 0.3f)
            .SetEase(Ease.InBack)
        );

        seq.OnComplete(() =>
        {
            timer.StartTimer();

            gameObject.SetActive(false);
        });
    }
}