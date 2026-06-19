using UnityEngine;
using UnityEngine.EventSystems;

public class SampleChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform panel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.gameObject.SetActive(false);
    }

}
