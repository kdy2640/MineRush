using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private TMP_Text endText;

    [SerializeField] private string message = "GAME OVER";

    [SerializeField] private float startY = 1200f;
    [SerializeField] private float centerY = 0f;
    [SerializeField] private float endY = -1200f;

    [SerializeField] private float enterDuration = 0.5f;
    [SerializeField] private float waitDuration = 3f;
    [SerializeField] private float exitDuration = 0.5f;

    private bool isPlaying;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        if (endText == null)
            endText = GetComponentInChildren<TMP_Text>();
    }

    public void Play()
    {
        if (isPlaying) return;

        gameObject.SetActive(true);

        StartCoroutine(PlayRoutine());
    }

    public IEnumerator PlayRoutine()
    {
        isPlaying = true;

        gameObject.SetActive(true);

        endText.text = message;

        target.anchoredPosition = new Vector2(0, startY);

        Sequence seq = DOTween.Sequence();

        seq.Append(
            target.DOAnchorPosY( centerY, enterDuration)
            .SetEase(Ease.OutBack));

        seq.AppendInterval(waitDuration);

        seq.Append(
            target.DOAnchorPosY( endY, exitDuration)
            .SetEase(Ease.InBack));

        yield return seq.WaitForCompletion();

        gameObject.SetActive(false);

        isPlaying = false;
    }
}