using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OrePresentor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer solidOreRenderer;

    [Header("Random Adjust")]
    [SerializeField] private float randomRotationRange = 25f;
    [SerializeField] private float randomScaleRange = 0.2f;

    [Header("Spawn Area")]
    [SerializeField] private float spawnHalfWidth = 0.35f;
    [SerializeField] private float spawnMinUpOffset = 0.25f;
    [SerializeField] private float spawnMaxUpOffset = 0.65f;

    [Header("Popup")]
    [SerializeField] private float popupDuration = 0.18f;
    [SerializeField] private float popupStartScale = 0.2f;
    [SerializeField] private float popupEndScale = 1f;
    [SerializeField] private float popupStayDuration = 0.1f;

    [Header("Move")]
    [SerializeField] private float moveDuration = 0.45f;
    [SerializeField] private float moveEndScale = 0.35f;

    private Sequence currentSequence;

    public void SetData(OreType type, Vector3 worldPosition)
    {
        StopCurrentTween();

        transform.position = worldPosition;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        solidOreRenderer.sprite = OreDataDB.GetOreDataSO(type).OreSprite;
        solidOreRenderer.gameObject.SetActive(false);

        ResetVisual();
        RandomAdjust();
    }

    private void ResetVisual()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

        solidOreRenderer.transform.localPosition = Vector3.zero;
        solidOreRenderer.transform.localRotation = Quaternion.identity;
        solidOreRenderer.transform.localScale = Vector3.one;

        Color color = solidOreRenderer.color;
        color.a = 1f;
        solidOreRenderer.color = color;
    }

    private void RandomAdjust()
    {
        float randomRotation = Random.Range(-randomRotationRange, randomRotationRange);
        float randomScale = Random.Range(1f - randomScaleRange, 1f + randomScaleRange);

        Vector3 randomSpawnOffset = new Vector3(
            Random.Range(-spawnHalfWidth, spawnHalfWidth),
            Random.Range(spawnMinUpOffset, spawnMaxUpOffset),
            0f
        );

        transform.position += randomSpawnOffset;

        solidOreRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, randomRotation);
        solidOreRenderer.transform.localScale = Vector3.one * randomScale;
    }

    public IEnumerator PopUpRoutine()
    {
        StopCurrentTween();

        solidOreRenderer.gameObject.SetActive(true);

        transform.localScale = Vector3.one * popupStartScale;

        currentSequence = DOTween.Sequence();

        currentSequence.Append(
            transform
                .DOScale(Vector3.one * popupEndScale, popupDuration)
                .SetEase(Ease.OutQuad)
        );

        currentSequence.AppendInterval(popupStayDuration);

        yield return currentSequence.WaitForCompletion();

        currentSequence = null;
    }

    public IEnumerator MoveToTarget(Vector3 targetWorldPosition)
    {
        StopCurrentTween();

        solidOreRenderer.gameObject.SetActive(true);

        currentSequence = DOTween.Sequence();

        currentSequence.Append(
            transform
                .DOMove(targetWorldPosition, moveDuration)
                .SetEase(Ease.Linear)
        );

        currentSequence.Join(
            transform
                .DOScale(Vector3.one * moveEndScale, moveDuration)
                .SetEase(Ease.Linear)
        );

        yield return currentSequence.WaitForCompletion();

        solidOreRenderer.gameObject.SetActive(false);

        currentSequence = null;
    }

    private void StopCurrentTween()
    {
        currentSequence?.Kill();
        currentSequence = null;
    }

    private void OnDestroy()
    {
        StopCurrentTween();
    }
}