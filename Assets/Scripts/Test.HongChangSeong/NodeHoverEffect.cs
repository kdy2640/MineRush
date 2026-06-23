using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class USceneChangePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //노드, 버튼들에 사운드 효과나 DOTween효과를 부여하는 스크립트가 될 예정.
    //이 스크립트를, 노드 프리팹 원본에다가 붙이면 끝일 것 같은데.
    //사운드는 GameManager.Instance.AudioManager.PlaySFX(SFXType.~~~) 이런 식으로 넣으면 나올 것.
    //enum 및 Inspector에 추가 필요.
    private Vector3 originalScale;
    private float hoverScale = 1.09f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) //Sequence를 쓰지 않을 것 같아서 간단히 구현.
    {        
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, 0.15f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        transform.DOKill();
        transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutQuad);
    }
}
