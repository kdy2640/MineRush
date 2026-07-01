using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class StonePresenter : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform solidObject;

    [Header("Spawn")]
    [SerializeField] private float spawnDuration = 0.2f;
    [SerializeField] private Ease spawnEase = Ease.OutBack;
    [SerializeField] private ParticleSystem breakParticle;
    [SerializeField] private float ReturnDelay = 2f;
    private Action OnBreakRouineEnd;

    private Tween currentTween;
    private Tween spawnTween;
    private Sequence hitSequence;
    private Sequence breakSequence;

    private Vector3 defaultSolidLocalPosition;
    private Vector3 defaultSolidLocalScale;

    private bool deathSequenceStarted = false;

    public bool DeathSequenceStarted
    {
        get { return deathSequenceStarted; }
        set { deathSequenceStarted = value; }
    }

    private void Awake()
    {
        if (solidObject == null)
            solidObject = transform;

        defaultSolidLocalPosition = solidObject.localPosition;
        defaultSolidLocalScale = solidObject.localScale;

        CreateTweens();
    }

    public void Initialize()
    {
        DeathSequenceStarted = false;

        StopCurrentTween();

        solidObject.localPosition = defaultSolidLocalPosition;
        solidObject.localScale = defaultSolidLocalScale;

        if (breakParticle != null)
        {
            breakParticle.transform.SetParent(solidObject, false);
            breakParticle.transform.localPosition = Vector3.zero;
        }
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
        spawnTween = solidObject
            .DOScale(defaultSolidLocalScale, spawnDuration)
            .SetEase(spawnEase)
            .SetAutoKill(false)
            .Pause();

        hitSequence = DOTween.Sequence()
            .Append(solidObject.DOShakePosition(0.12f, 0.08f, 10))
            .SetAutoKill(false)
            .Pause();

        breakSequence = DOTween.Sequence()
            .Append(solidObject.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack))
            .SetAutoKill(false)
            .Pause();
    }

    // 생성용
    public void PlaySpawnTween()
    {
        StartCoroutine(PlaySpawnRoutine());
    }

    // 반환용
    public IEnumerator PlaySpawnRoutine()
    {
        StopCurrentTween();


        if (breakParticle != null)
        {
            breakParticle.transform.SetParent(null, true); 
        }

        solidObject.localPosition = defaultSolidLocalPosition;
        solidObject.localScale = Vector3.zero;

        currentTween = spawnTween;
        spawnTween.Restart();

        yield return spawnTween.WaitForCompletion();

        if (currentTween == spawnTween)
            currentTween = null;
    }

    public IEnumerator PlayHitReactionRoutine()
    {
        StopCurrentTween();

        solidObject.localPosition = defaultSolidLocalPosition;

        GameManager.Instance.AudioManager.PlaySFXRandomPitch(SFXType.StoneHit, 0.2f);

        currentTween = hitSequence;
        hitSequence.Restart();

        yield return hitSequence.WaitForCompletion();

        solidObject.localPosition = defaultSolidLocalPosition;

        if (currentTween == hitSequence)
            currentTween = null;
    }

    public IEnumerator PlayBreakRoutine()
    {
        StopCurrentTween();

        GameManager.Instance.AudioManager.PlaySFXRandomPitch(SFXType.StoneCrush, 0.2f);

        if (breakParticle != null)
        {
            breakParticle.Play();
        }

        currentTween = breakSequence;
        breakSequence.Restart();

        yield return new WaitForSeconds(ReturnDelay);


        if (currentTween == breakSequence)
            currentTween = null;

        OnBreakRouineEnd?.Invoke();
    }

    public void StopCurrentTween()
    {
        if (currentTween == null)
            return;

        currentTween.Pause();
        currentTween.Rewind();

        solidObject.localPosition = defaultSolidLocalPosition;

        currentTween = null;
    }

    public void SubscribeBreakRoutineEnd(Action ev)
    {
        OnBreakRouineEnd += ev;
    }

    public void UnSubscribeBreakRoutineEnd(Action ev)
    {
        OnBreakRouineEnd -= ev;
    }
}