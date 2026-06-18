using DG.Tweening;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform startRoot;
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
        startRoot.anchoredPosition =
            new Vector2(0, 500);

        canvasGroup.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        seq.Append(startRoot.DOAnchorPosY(-20, 0.5f).SetEase(Ease.OutCubic));

        seq.Append(startRoot.DOAnchorPosY(15, 0.15f).SetEase(Ease.OutQuad));

        seq.Append(startRoot.DOAnchorPosY(0, 0.15f).SetEase(Ease.OutBounce));

        seq.Append(startText.DOShakeRotation(0.25f,5f,20));

        seq.AppendInterval(1.2f);

        seq.Append(startRoot.DOAnchorPosY(500,0.4f).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            timer.StartTimer();
            controller.StartGame();

            gameObject.SetActive(false);
        });
    }
}