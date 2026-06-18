using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class USceneChangePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private float hoverScale = 1.09f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
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
