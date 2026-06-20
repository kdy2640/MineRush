using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform title;
    [SerializeField] private CanvasGroup group;

    public Action OnFinished;

    public void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        group.alpha = 1;

        title.anchoredPosition = new Vector2(0, 400);

        Sequence seq = DOTween.Sequence();

        seq.Append(title.DOAnchorPosY(0, 0.5f));
        seq.AppendInterval(1f);
        seq.Append(title.DOAnchorPosY(400, 0.4f));

        yield return seq.WaitForCompletion();

        gameObject.SetActive(false);

        OnFinished?.Invoke();
    }
}