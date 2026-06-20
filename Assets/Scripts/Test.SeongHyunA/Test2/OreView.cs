using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OreView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] breakSprites;

    public void PlayBreak(Action onComplete)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(0.85f, 0.1f));
        seq.Append(transform.DOShakePosition(0.2f, 0.2f));

        seq.AppendCallback(() =>
        {
            if (breakSprites.Length > 0)
                sr.sprite = breakSprites[0];
        });

        seq.AppendInterval(0.1f);

        seq.AppendCallback(() =>
        {
            if (breakSprites.Length > 1)
                sr.sprite = breakSprites[1];
        });

        seq.AppendInterval(0.1f);

        seq.Append(sr.DOFade(0f, 0.3f));

        seq.OnComplete(() => onComplete?.Invoke());
    }
}