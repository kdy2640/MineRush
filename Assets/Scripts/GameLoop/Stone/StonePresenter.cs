using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StonePresenter : MonoBehaviour
{
    private Tween currentTween;

    public IEnumerator PlayHitReactionRoutine()
    {
        StopCurrentTween();

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOShakePosition(0.12f, 0.08f, 10));

        currentTween = seq;

        yield return seq.WaitForCompletion();

        currentTween = null;
    }

    public IEnumerator PlayBreakRoutine()
    {
        StopCurrentTween();

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(Vector3.zero, 0.2f));

        currentTween = seq;

        yield return seq.WaitForCompletion();

        currentTween = null;
    }

    public void StopCurrentTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }

    private void OnDestroy()
    {
        StopCurrentTween();
    }
}
