using DG.Tweening;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private OreType oreType;
    
    public OreType OreType 
    {
        get { return oreType; }
    }

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }
    public void PlayBreakWeen()
    {
        transform.DOKill();

        Sequence seq = DOTween.Sequence();

        // 1. 살짝 눌림
        seq.Append(transform.DOScale(originalScale * 0.8f, 0.1f)
            .SetEase(Ease.OutQuad));

        // 2. 마구 흔들림
        seq.Append(transform.DOShakePosition(0.15f, 0.25f, vibrato: 20));

        // 3. 비틀리며 흔들림
        seq.Join(transform.DOShakeRotation(0.2f, 20f));

        // 4. 원래 크기 복귀 + 추가 흔들림
        seq.Append(transform.DOScale(originalScale, 0.12f)
            .SetEase(Ease.OutBack));

        seq.Append(transform.DOShakePosition(0.1f, 0.1f));

        // 5. 비활성화
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }


}
