using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PickaxeActor : Poolable
{
    [Header("Position")]
    [SerializeField] private Vector3 deltaPosition = new Vector3(0.25f,0.1f,0);

    [Header("Rotation Angle")]
    [SerializeField] private float backAngle = -25f;
    [SerializeField] private float forwardAngle = 50f;

    [Header("Tween")]
    [SerializeField] private float swingDuration = 0.12f;
    [SerializeField] private float returnDuration = 0.22f;

    [SerializeField] private Ease swingEase = Ease.InQuad;
    [SerializeField] private Ease returnEase = Ease.OutSine;

    private SpriteRenderer renderer;

    private Sequence rightSequence;
    private Sequence leftSequence;
    private Sequence currentSequence;

    private float baseX;
    private float baseY;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();

        Vector3 euler = transform.localEulerAngles;
        baseX = euler.x;
        baseY = euler.y;

        rightSequence = CreateAttackSequence(1);
        leftSequence = CreateAttackSequence(-1);
    }

    private Sequence CreateAttackSequence(int offset)
    {
        float startAngle = backAngle * offset;
        float attackAngle = forwardAngle * offset;

        Sequence seq = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause();

        seq.Append(transform.DOLocalRotate(
            new Vector3(baseX, baseY, attackAngle),
            swingDuration,
            RotateMode.Fast
        ).SetEase(swingEase));

        seq.Append(transform.DOLocalRotate(
            new Vector3(baseX, baseY, startAngle),
            returnDuration,
            RotateMode.Fast
        ).SetEase(returnEase));

        return seq;
    }

    public IEnumerator PlayAttackRoutine(Vector3 summonPosition)
    {
        StopCurrentTween();

        int offset = Random.value > 0.5f ? -1 : 1;

        transform.position = summonPosition + new Vector3(
            offset * deltaPosition.x,
            deltaPosition.y,
            deltaPosition.z
        );

        transform.localScale = new Vector3(offset, 1f, 1f);

        float startAngle = backAngle * offset;
        transform.localRotation = Quaternion.Euler(baseX, baseY, startAngle);

        currentSequence = offset == 1 ? rightSequence : leftSequence;

        currentSequence.Restart();

        yield return currentSequence.WaitForCompletion();

        currentSequence = null;
         
        RequestReturn(); 
    }

    public void StopCurrentTween()
    {
        if (currentSequence == null)
            return;

        currentSequence.Pause();
        currentSequence.Rewind();

        currentSequence = null;
    }

    private void OnDisable()
    {
        StopCurrentTween();
    }

    private void OnDestroy()
    {
        rightSequence?.Kill();
        leftSequence?.Kill();

        rightSequence = null;
        leftSequence = null;
        currentSequence = null;
    }

    public override void Initialize(PoolArgs obj)
    {
        StopCurrentTween();
        renderer.sprite = PickaxeDataDB.GetStoneDataSO(GameManager.Instance.Upgrade.GetRuntimeStat().PickaxeTier).Icon;
    }

    public override void ResetState()
    {
        StopCurrentTween();

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(baseX, baseY, 0f);
    }
}