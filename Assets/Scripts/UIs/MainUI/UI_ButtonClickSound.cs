using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonClickSound : MonoBehaviour, IPointerEnterHandler
{
    Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFXType.ButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.AudioManager.PlaySFX(SFXType.UIHover);
    }
}

