using System;
using DG.Tweening;
using UnityEngine;

public class OreView : MonoBehaviour
{
    public void PlayCollect()
    {
        transform.DOKill();

        transform.DOShakeScale( 0.15f, 0.2f);
    }

    public void PlayDisappear(
        Action onComplete = null)
    {
        transform.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append( transform.DOShakeScale( 0.15f, 0.2f));

        seq.Append( transform.DOScale( 0f, 0.25f));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void ResetView()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.one;

        transform.rotation = Quaternion.identity;
    }
}