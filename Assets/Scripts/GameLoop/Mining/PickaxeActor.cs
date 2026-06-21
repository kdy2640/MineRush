using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PickaxeActor : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Vector3 deltaPositon = Vector3.zero;
    [SerializeField] private bool isFlipx = false;

    [Header("Rotation Angle")]
    [SerializeField] private float backAngle = -25f;
    [SerializeField] private float forwardAngle = 50f;

    [Header("Tween")]
    [SerializeField] private float swingDuration = 0.12f;
    [SerializeField] private float returnDuration = 0.22f;

    [SerializeField] private Ease swingEase = Ease.InQuad;
    [SerializeField] private Ease returnEase = Ease.OutSine;

    private Tween currentTween;

    private float baseX;
    private float baseY;

    private void Awake()
    {
        Vector3 euler = transform.localEulerAngles;
        baseX = euler.x;
        baseY = euler.y;
    }

    public IEnumerator PlayAttackRoutine(Vector3 summonPosition)
    {
        StopCurrentTween();

        int offset = 1;

        isFlipx = Random.value > 0.5f;

        if (isFlipx)
        {
            offset = -1;
        }

        transform.position = summonPosition + new Vector3(
            offset * deltaPositon.x,
            deltaPositon.y,
            deltaPositon.z
        );

        transform.localScale = new Vector3(offset, 1, 1);

        float startAngle = backAngle * offset;
        float attackAngle = forwardAngle * offset;

        transform.localRotation = Quaternion.Euler(baseX, baseY, startAngle);

        Sequence seq = DOTween.Sequence();

        // 앞으로 휘두르기
        seq.Append(transform.DOLocalRotate(
            new Vector3(baseX, baseY, attackAngle),
            swingDuration,
            RotateMode.Fast
        ).SetEase(swingEase));

        // 다시 뒤로 복귀
        seq.Append(transform.DOLocalRotate(
            new Vector3(baseX, baseY, startAngle),
            returnDuration,
            RotateMode.Fast
        ).SetEase(returnEase));

        currentTween = seq;

        yield return seq.WaitForCompletion();

        currentTween = null;

        Destroy(gameObject);
    }

    public void StopCurrentTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }

    private void OnDisable()
    {
        StopCurrentTween();
    }

    private void OnDestroy()
    {
        StopCurrentTween();
    }
}