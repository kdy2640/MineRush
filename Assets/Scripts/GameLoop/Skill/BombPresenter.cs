using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BombPresenter : MonoBehaviour, IAttackPresenter
{
    [SerializeField] SpriteFlipbookPlayer bombFlipBook;
    [SerializeField] SpriteRenderer solidBomb;

    [Header("Timing")]
    [SerializeField] float popupTime = 0.12f;
    [SerializeField] float pulseInTime = 0.18f;
    [SerializeField] float pulseOutTime = 0.18f;

    [Header("Scale")]
    [SerializeField] float startScale = 0.4f;
    [SerializeField] float normalScale = 1.0f;
    [SerializeField] float pulseScale = 1.18f;

    private Sequence bombSequence;

    public IEnumerator PlayAttackRoutine(Vector3 targetPosition)
    {
        transform.position = targetPosition;

        Transform bombTransform = solidBomb.transform;

        bombSequence?.Kill();

        solidBomb.gameObject.SetActive(true);
        bombTransform.localScale = Vector3.one * startScale;

        bombSequence = DOTween.Sequence()
            .Append(bombTransform.DOScale(normalScale, popupTime).SetEase(Ease.OutBack))
            .Append(bombTransform.DOScale(pulseScale, pulseInTime * 0.5f).SetEase(Ease.OutQuad))
            .Append(bombTransform.DOScale(normalScale, pulseOutTime * 0.5f).SetEase(Ease.InQuad))
            .Append(bombTransform.DOScale(pulseScale, pulseInTime * 0.5f).SetEase(Ease.OutQuad))
            .Append(bombTransform.DOScale(normalScale, pulseOutTime * 0.5f).SetEase(Ease.InQuad));

        yield return bombSequence.WaitForCompletion();

        solidBomb.gameObject.SetActive(false);

        GameManager.Instance.AudioManager.PlaySFX(SFXType.Explosion);

        yield return bombFlipBook.PlayRoutine();

        // 여기부터는 플립북 끝난 뒤 로직
    }

    private void OnDisable()
    {
        bombSequence?.Kill();
        bombSequence = null;
    }
}