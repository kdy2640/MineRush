using DG.Tweening;
using UnityEngine;

public class PickAxesPanel : MonoBehaviour
{
    //Inspector 상에서 content를 할당한다.
    [SerializeField] private RectTransform content;

    [SerializeField] private float moveDistance = 200f;
    [SerializeField] float moveDuration = 0.3f;

    private int currentIndex = 0; //0에 가까울수록 Left버튼을 많이 누른 상태. 클수록 Right버튼을 많이 누른 상태

    public void MoveLeft() //왼쪽으로 움직일 때. Button의 Onclick에 등록하여 사용?
    {
        if (currentIndex <= 0) return;

        currentIndex--; //움직임 인덱스에서 1 감소

        content.DOAnchorPosX(-currentIndex * moveDistance, moveDuration);
        //현재 보여줘야 할 위치를 계산한다. 둘 다 -인 것과는 관계가 없다고 함.

    }

    public void MoveRight() //오른쪽으로 움직일 때.
    {
        int clickLimit = PickaxeDataDB.GetPickaxeKeyCount();

        if (currentIndex >= clickLimit) return;

        currentIndex++;

        content.DOAnchorPosX(-currentIndex * moveDistance, moveDuration);
    }

}
