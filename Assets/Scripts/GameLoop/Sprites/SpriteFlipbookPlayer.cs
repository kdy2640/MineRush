using System.Collections;
using UnityEngine;

public class SpriteFlipbookPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Frames")]
    [SerializeField] private Sprite[] frames;

    [Header("Playback")]
    [SerializeField] private float fps = 12f;
    [SerializeField] private bool playOnEnable = false;
    [SerializeField] private bool deactivateOnComplete = false;

    [Header("Impact")]
    [SerializeField] private int impactIndex = 0;
     
    private Coroutine afterImpactCoroutine;

    public bool IsPlaying =>   afterImpactCoroutine != null;
    public int FrameCount => frames == null ? 0 : frames.Length;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
     
    private void OnDisable()
    {
        Stop();
    }

    public void SetFrames(Sprite[] newFrames)
    {
        frames = newFrames;
    }

    public void SetImpactIndex(int index)
    {
        impactIndex = index;
    }

    public void Stop()
    { 

        if (afterImpactCoroutine != null)
        {
            StopCoroutine(afterImpactCoroutine);
            afterImpactCoroutine = null;
        }
    }

    public void ResetToFirstFrame()
    {
        if (frames == null || frames.Length == 0)
            return;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = frames[0];
    }

    public IEnumerator PlayRoutine()
    {
        Stop();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError($"{nameof(SpriteFlipbookPlayer)}: SpriteRenderer가 없습니다.", this);
            yield break;
        }

        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning($"{nameof(SpriteFlipbookPlayer)}: 재생할 프레임이 없습니다.", this);

            if (deactivateOnComplete)
                gameObject.SetActive(false);

            yield break;
        }
         
        yield return PlayUntilImpactRoutine();
    }

    private IEnumerator PlayUntilImpactRoutine()
    {
        float delay = fps <= 0f ? 0.1f : 1f / fps;

        int clampedImpactIndex = Mathf.Clamp(impactIndex, 0, frames.Length - 1);

        for (int i = 0; i <= clampedImpactIndex; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        } 

        int nextIndex = clampedImpactIndex + 1;

        if (nextIndex < frames.Length)
        {
            afterImpactCoroutine = StartCoroutine(PlayAfterImpactRoutine(nextIndex));
        }
        else
        {
            Complete();
        }
    }

    private IEnumerator PlayAfterImpactRoutine(int startIndex)
    {
        float delay = fps <= 0f ? 0.1f : 1f / fps;

        for (int i = startIndex; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }

        afterImpactCoroutine = null;
        Complete();
    }

    private void Complete()
    {
        if (deactivateOnComplete)
            gameObject.SetActive(false);
    }
}