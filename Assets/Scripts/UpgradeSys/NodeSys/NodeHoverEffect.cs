using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //노드, 버튼들에 사운드 효과나 DOTween효과를 부여하는 스크립트가 될 예정.
    //이 스크립트를, 노드 프리팹 원본에다가 붙이면 될 것 같음.
    //사운드는 GameManager.Instance.AudioManager.PlaySFX(SFXType.~~~) 이런 식으로 넣으면 나올 것.
    //enum 및 Inspector에 추가 필요.
    private Vector3 originalScale;
    private float hoverScale = 1.09f;

    private Tween zoomInTween;
    private Tween zoomOutTween;

    private void Awake()
    {
        originalScale = transform.localScale;
        zoomInTween = transform
            .DOScale(hoverScale, 0.15f)
            .SetEase(Ease.OutQuad)
            .Pause()
            .SetAutoKill(false);
        zoomOutTween = transform
            .DOScale(originalScale, 0.15f)
            .SetEase(Ease.OutQuad)
            .Pause()
            .SetAutoKill(false);
    }

    public void OnPointerEnter(PointerEventData eventData) //Sequence를 쓰지 않을 것 같아서 간단히 구현.
    {
        GameManager.Instance.AudioManager.PlaySFX(SFXType.UIHover);
        zoomOutTween.Pause();
        zoomInTween.Restart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        zoomInTween.Pause();
        zoomOutTween.Restart();
    }
    private void OnDestroy()
    {
        zoomInTween?.Kill();
        zoomOutTween?.Kill();
    }
}
