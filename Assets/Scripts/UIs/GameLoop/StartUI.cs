using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform target;

    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private float enterDuration = 0.5f;

    [SerializeField] private float exitDuration = 0.5f;

    [SerializeField] private float startY = 1200f;

    [SerializeField] private float centerY = 0f;

    [SerializeField] private string startMessage = "Mining Start!";

    public UnityEvent onFinished;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        target.anchoredPosition = new Vector2(0, startY);

        Sequence seq = DOTween.Sequence();

        seq.Append(
            target.DOAnchorPosY( centerY, enterDuration)
            .SetEase(Ease.OutBack));

        yield return seq.WaitForCompletion();

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = startMessage;

        yield return new WaitForSeconds(1f);

        seq = DOTween.Sequence();

        seq.Append(
            target.DOAnchorPosY( startY, exitDuration)
            .SetEase(Ease.InBack));

        yield return seq.WaitForCompletion();

        gameObject.SetActive(false);

        onFinished?.Invoke();
    }
}