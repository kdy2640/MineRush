using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StonePresenter : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float spawnDuration = 0.2f;
    [SerializeField] private Ease spawnEase = Ease.OutBack;
    [SerializeField] private ParticleSystem breakParticle;

    private Tween currentTween;

    private Tween spawnTween;
    private Sequence hitSequence;
    private Sequence breakSequence;

    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;

        CreateTweens();
    }

    private void OnDestroy()
    {
        currentTween?.Kill();

        spawnTween?.Kill();
        hitSequence?.Kill();
        breakSequence?.Kill();
    }

    private void CreateTweens()
    {
        spawnTween = transform
            .DOScale(defaultScale, spawnDuration)
            .SetEase(spawnEase)
            .SetAutoKill(false)
            .Pause();

        hitSequence = DOTween.Sequence()
            .Append(transform.DOShakePosition(0.12f, 0.08f, 10))
            .SetAutoKill(false)
            .Pause();

        breakSequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack))
            .SetAutoKill(false)
            .Pause();
    }

    public IEnumerator PlaySpawnRoutine()
    {
        StopCurrentTween();

        transform.localScale = Vector3.zero;

        currentTween = spawnTween;
        spawnTween.Restart();

        yield return spawnTween.WaitForCompletion();

        if (currentTween == spawnTween)
            currentTween = null;
    }

    public IEnumerator PlayHitReactionRoutine()
    {
        StopCurrentTween();

        GameManager.Instance.AudioManager.PlaySFXRandomPitch(SFXType.StoneHit, 0.2f); 

        currentTween = hitSequence;
        hitSequence.Restart();
        
        yield return hitSequence.WaitForCompletion();

        if (currentTween == hitSequence)
            currentTween = null;
    }

    public IEnumerator PlayBreakRoutine()
    {
        StopCurrentTween();

        if (breakParticle != null)
        {
            breakParticle.transform.SetParent(null, true);
        }

        GameManager.Instance.AudioManager.PlaySFXRandomPitch(SFXType.StoneCrush,0.2f);

        currentTween = breakSequence;
        breakSequence.Restart();

        yield return breakSequence.WaitForCompletion();

        if (breakParticle != null)
            breakParticle.Play();

        if (currentTween == breakSequence)
            currentTween = null;
    }

    public void StopCurrentTween()
    {
        if (currentTween == null)
            return;

        currentTween.Pause();
        currentTween.Rewind();

        currentTween = null;
    }
}