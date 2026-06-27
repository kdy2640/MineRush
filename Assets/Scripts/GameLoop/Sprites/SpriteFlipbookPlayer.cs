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

    private Coroutine playCoroutine;

    public bool IsPlaying => playCoroutine != null;
    public int FrameCount => frames == null ? 0 : frames.Length;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (playOnEnable)
            playCoroutine = StartCoroutine(PlayRoutine());
    }

    private void OnDisable()
    {
        Stop();
    }

    public void SetFrames(Sprite[] newFrames)
    {
        frames = newFrames;
    }

    public void Stop()
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
    }

    public void ResetToFirstFrame()
    {
        if (frames == null || frames.Length == 0)
            return;

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
        yield return PlayInternalRoutine();
    }

    private IEnumerator PlayInternalRoutine()
    {
        float delay = fps <= 0f ? 0.1f : 1f / fps;

        for (int i = 0; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }

        playCoroutine = null;

        if (deactivateOnComplete)
            gameObject.SetActive(false);
    }
}