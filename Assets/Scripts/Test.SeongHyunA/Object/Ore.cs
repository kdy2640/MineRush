using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private OreType oreType;
    [SerializeField] private Sprite[] sprites; // 교체할 이미지 파일들

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    public OreType OreType
    {
        get { return oreType; }
    }

    private void Awake()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        PlayBreakTween();
    }

    public void PlayBreakTween()
    {
        // 중복 실행 방지 및 초기화
        transform.DOKill();
        transform.localScale = originalScale;

        Sequence seq = DOTween.Sequence();

        // 1. 살짝 눌림 (0.1초)
        seq.Append(transform.DOScale(originalScale * 0.8f, 0.1f)
            .SetEase(Ease.OutQuad));

        // 2. 마구 흔들림 (0.15초)
        seq.Append(transform.DOShakePosition(0.15f, 0.25f, vibrato: 20));

        // 3. 비틀리며 흔들림 (0.2초 동안 같이 진행)
        seq.Join(transform.DOShakeRotation(0.2f, 20f));

        // ★ [애니메이션 중간 타이밍] 원래 크기로 복귀하기 직전에 스프라이트 변경!
        // sprites 배열에 등록된 첫 번째 이미지로 교체합니다.
        seq.AppendCallback(() =>
        {
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0];
            }
        });

        // 4. 원래 크기 복귀 (0.12초)
        seq.Append(transform.DOScale(originalScale, 0.12f)
            .SetEase(Ease.OutBack));

        // ★ [추가 타이밍] 마지막 흔들림 직전에 두 번째 이미지로 교체하고 싶다면 사용
        seq.AppendCallback(() =>
        {
            if (sprites != null && sprites.Length > 1)
            {
                spriteRenderer.sprite = sprites[1];
            }
        });

        // 5. 마지막 흔들림 (0.1초)
        seq.Append(transform.DOShakePosition(0.1f, 0.1f));

        // 6. 모든 연출이 끝난 후 비활성화
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
//{
//    [SerializeField] private OreType oreType;
//    [SerializeField] private Sprite[] sprites;
//    private SpriteRenderer spriteRenderer;
//    private Vector3 originalScale;

//    public OreType OreType 
//    {
//        get { return oreType; }
//    }


//    private void Awake()
//    {
//        originalScale = transform.localScale;
//        spriteRenderer = GetComponent<SpriteRenderer>();
//    }
//    private void OnMouseDown()
//    {
//        PlayBreakWeen();
//        StartCoroutine(ChangeSpriteCoroutine());
//    }
//    public void PlayBreakWeen()
//    {
//        transform.DOKill();

//        Sequence seq = DOTween.Sequence();

//        // 1. 살짝 눌림
//        seq.Append(transform.DOScale(originalScale * 0.8f, 0.1f)
//            .SetEase(Ease.OutQuad));

//        // 2. 마구 흔들림
//        seq.Append(transform.DOShakePosition(0.15f, 0.25f, vibrato: 20));

//        // 3. 비틀리며 흔들림
//        seq.Join(transform.DOShakeRotation(0.2f, 20f));

//        // 4. 원래 크기 복귀 + 추가 흔들림
//        seq.Append(transform.DOScale(originalScale, 0.12f)
//            .SetEase(Ease.OutBack));

//        seq.Append(transform.DOShakePosition(0.1f, 0.1f));

//        // 5. 비활성화
//        seq.OnComplete(() =>
//        {
//            gameObject.SetActive(false);
//        });
//    }
//    private IEnumerator ChangeSpriteCoroutine()
//    {
//        foreach(var sprite in sprites)
//        {
//            spriteRenderer.sprite = sprite;
//            yield return new WaitForSeconds(0.1f);
//        }
//    }

//}
