using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

//0625 홍창성

//곡괭이가 제작 완료됐을 때의 연출을 담당하는 스크립트.
//이 스크립트가 붙을 UI는 아마도 화면 상에는 존재하지만, 비활성화상태로 있고 제작이 완료됐을 때만 활성화될 것이다.
//제작 완료 시 = 매개변수로 이미지를 전달받아 이 스크립트가 붙은 이미지의 스프라이트를 currentPickaxe.icon 이런 식으로 받아서
//교체하게 될 것이다.
//그러면, 이 녀석을 외부에서 호출할 녀석은 곡괭이 찾아오는 거 또 해야겠지.

public class PickaxeBuyDisplay : MonoBehaviour
{

    //NodeHoverEffect에선 transform 써서 했는데 대체 뭔 차이가 있는 거고 그 여파는 뭐지?

    //Scale, Rotation, Punch, Shake 효과는 transform을 써도 문제 없다.
    //위치 이동이나 크기 변경은 RectTransform의 DOAnchorPos, DOSizeDelta를 사용하는 것이 좋다.
    //UI용 코드라는 의도를 명확히 하기 위해서라도 UI 스크립트에서는 보통 RectTransform rectTransform을 캐싱해서 사용한다. 

    [Header("구매 완료 연출")]
    [SerializeField] private Image pickaxeIconImg;
    [SerializeField] private RectTransform pickaxeIconRect;
    [SerializeField] private Image pickaxeIconGlowRayImg;
    [SerializeField] private float extraScale = 15f;
    [SerializeField] private UnityEvent onDisplayComplete;
    private Vector3 originalScale;
    private Sequence buySequence;


    //아래의 displayBuying 메서드를 사용할 다른 스크립트에서 구매한 곡괭이 이미지를 전달하기 위한 프로퍼티.
    //아마도 아래의 private Image image를 삭제해도 될 듯.

    ////이거를 
    //[field: SerializeField] public Image PickaxeImage;

    
    [Header("마우스 홀딩 관련")]
    [SerializeField] private GameObject holdingMouseUI;
    [SerializeField] private Image holdingMouseFillImage;

    

    private void Awake()
    {
        originalScale = pickaxeIconRect.localScale;

        SetOriginal();
        pickaxeIconRect.gameObject.SetActive(false);
    }

    private void OnEnable() //테스트를 위해 OnEnable에 넣어봄.
    {
    }

    private void OnDisable()
    {
        SetOriginal();
    }

    public void SetImg(PickaxesDataSO data)
    {
        if (data == null)
            return;

        pickaxeIconImg.sprite = data.Icon;
    } // 전달받은 곡괭이 데이터의 아이콘을 구매 완료 연출 이미지에 세팅한다.


    // public void DisplayBuying() //이 메서드를 원하는 상황에 원하는 오브젝트에 붙여 호출하기만 하면 될 것 같은데.
    // {
    //     //이 메서드를 외부에서 호출하는 녀석이 있다면, 그 녀석이 Sprite pickaxeSprite로 매개변수를 전달하기만 하면 되겠지.
    //
    //     
    //
    //     //image.sprite = pickaxeSprite;
    //
    //     Sequence sequence = DOTween.Sequence();
    //
    //     //레퍼런스 게임과 같이 완전히 작아진 상태에서 커지는 방식으로 만들 거라면 From을 써야 한다.
    //     sequence.Append(pickaxeIconRect.DOScale(originalScale, duration).From(Vector3.zero).SetEase(Ease.OutBack));
    //
    //     sequence.Append(pickaxeIconRect.DOScale(originalScale * extraScale, duration).SetEase(Ease.OutQuad));
    //
    //     sequence.Join(pickaxeIconImg.DOFade(0.0f, duration));
    //
    //     //연출이 전부 끝난 후 스스로를 꺼버리게?
    //
    //     // enabled = false;
    //
    //     // 손유민 : 연출이 끝난 후 끄게 하려면 시퀀스의 콜백을 이용하시면 됩니다.
    //     sequence.OnComplete(() =>
    //     {
    //         gameObject.SetActive(false);
    //         onDisplayComplete?.Invoke();
    //     });
    // }

    public void SetOriginal()
    {
        buySequence?.Kill();
        pickaxeIconRect.DOKill();
        pickaxeIconImg.DOKill();

        pickaxeIconRect.localScale = originalScale;

        pickaxeIconGlowRayImg.GetComponent<RectTransform>().localScale = originalScale;
        Color tempColor = pickaxeIconGlowRayImg.color;
        pickaxeIconGlowRayImg.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
        tempColor = pickaxeIconImg.color;
        pickaxeIconImg.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1f);
    } // 연출에 사용된 크기와 알파값을 초기 상태로 되돌린다.

    public void PlayBuyCompleteVisual(PickaxesDataSO data)
    {
        SetImg(data);
        buySequence?.Kill();

        pickaxeIconRect.gameObject.SetActive(true);
        SetOriginal();
        
        buySequence = DOTween.Sequence();
        Sequence pickaxeIconSeq = DOTween.Sequence();
        
        pickaxeIconSeq.Append(pickaxeIconRect
            .DOScale(originalScale, 0.7f)
            .From(Vector3.zero)
            .SetEase(Ease.OutBack));
        //아이콘 확대
        
        pickaxeIconSeq.Append(pickaxeIconRect
            .DOScale(originalScale * extraScale, 0.7f)
            .SetEase(Ease.OutQuad).SetDelay(1.3f));
        pickaxeIconSeq.Join(pickaxeIconImg
            .DOFade(0f, 0.7f)
            .From(1f));
        //아이콘 확대 및 아이콘 페이드
        pickaxeIconSeq.OnComplete(() =>
        {
            pickaxeIconRect.gameObject.SetActive(false);
            onDisplayComplete?.Invoke();
        });

        
        
        Sequence glowRaySeq = DOTween.Sequence();
        glowRaySeq.Append(
            pickaxeIconGlowRayImg
                .DOFade(1f, 0.57f)
                .From(0f)
                .SetEase(Ease.InQuad));
        glowRaySeq.Join(
                pickaxeIconGlowRayImg.GetComponent<RectTransform>()
                    .DOScale(originalScale, 0.57f)
                    .From(Vector3.zero)
                    .SetEase(Ease.InQuad));
        
        glowRaySeq.Append(pickaxeIconGlowRayImg
            .DOFade(0f, 2f)
            .SetEase(Ease.InQuad));
        
        buySequence.Append(pickaxeIconSeq);
        buySequence.Join(glowRaySeq);
    }


    public void ShowHoldingUI(bool isActive)
    {
        holdingMouseUI.SetActive(isActive);
    }
    public void SetHoldingFillAmount(float progress)
    {
        holdingMouseFillImage.fillAmount = progress;
    }
}
