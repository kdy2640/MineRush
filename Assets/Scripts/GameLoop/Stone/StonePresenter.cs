using DG.Tweening;
using System.Collections; 
using UnityEngine;

public class StonePresenter : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float spawnDuration = 0.2f;
    [SerializeField] private Ease spawnEase = Ease.OutBack;
     

    private Tween currentTween;  
    private void OnDestroy()
    {  
        StopCurrentTween();
    }
    public IEnumerator PlaySpawnRoutine()
    {
        StopCurrentTween();

        transform.localScale = Vector3.zero;

        currentTween = transform
            .DOScale(Vector3.one, spawnDuration)
            .SetEase(spawnEase);

        yield return currentTween.WaitForCompletion();

        currentTween = null;
    }

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

        seq.Append(transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack));

        currentTween = seq;

        yield return seq.WaitForCompletion();

        currentTween = null;
    }

    public void StopCurrentTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }
     
}