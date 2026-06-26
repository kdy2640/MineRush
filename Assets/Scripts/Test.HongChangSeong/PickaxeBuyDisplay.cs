using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//0625 홍창성

public class PickaxeBuyDisplay : MonoBehaviour
{

    //NodeHoverEffect에선 transform 써서 했는데 대체 뭔 차이가 있는 거고 그 여파는 뭐지?

    //Scale, Rotation, Punch, Shake 효과는 transform을 써도 문제 없다.
    //위치 이동이나 크기 변경은 RectTransform의 DOAnchorPos, DOSizeDelta를 사용하는 것이 좋다.
    //UI용 코드라는 의도를 명확히 하기 위해서라도 UI 스크립트에서는 보통 RectTransform rectTransform을 캐싱해서 사용한다. 

    [SerializeField] private float extraScale = 15f;
    [SerializeField] private float duration = 0.5f;

    private Vector3 originalScale;
    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable() //테스트를 위해 OnEnable에 넣어봄.
    {
        SetOriginal();

        DisplayBuying();
    }

 

    public void DisplayBuying() //이 메서드를 원하는 상황에 원하는 오브젝트에 붙여 호출하기만 하면 될 것 같은데.
    {
        Sequence sequence = DOTween.Sequence();

        //레퍼런스 게임과 같이 완전히 작아진 상태에서 커지는 방식으로 만들 거라면 From을 써야 한다.
        sequence.Append(rectTransform.DOScale(originalScale, duration).From(Vector3.zero).SetEase(Ease.OutBack));

        sequence.Append(rectTransform.DOScale(originalScale * extraScale, duration).SetEase(Ease.OutQuad));

        sequence.Join(image.DOFade(0.0f, duration));
    }

    public void SetOriginal() //알파값과 크기 등등을 원래대로 바꿔버리는 메서드.
    {
        rectTransform.localScale = originalScale;

        image.DOKill();

        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }
}
