using DG.Tweening;
using UnityEngine;

public class PickAxesPanel : MonoBehaviour
{
    //Inspector 상에서 content를 할당한다.
    [SerializeField] private RectTransform content;

    [SerializeField] private float moveDistance = 200f;
    [SerializeField] float moveDuration = 0.3f;

    [SerializeField] private CurrentPickaxeInfo pickaxeInfo;

    private int currentIndex = 0; //0에 가까울수록 Left버튼을 많이 누른 상태. 클수록 Right버튼을 많이 누른 상태


    private void Update()
    {
        ShowPickaxeInfo(SetCurrentPickaxe(currentIndex)); //0으로 초기화를 해뒀기에 처음에는 기본 곡괭이가 나온다.
        //근데, 메서드 안에 메서드가 있는 거 기묘한데 뭔가 아닌 것 같기도 하고.
    }

    public void MoveLeft() //왼쪽으로 움직일 때. Button의 Onclick에 등록하여 사용.
    {
        if (currentIndex <= 0) return;

        currentIndex--; //움직임 인덱스에서 1 감소

        content.DOAnchorPosX(-currentIndex * moveDistance, moveDuration);
        //현재 보여줘야 할 위치를 계산한다. 둘 다 -인 것과는 관계가 없다고 함.

        //ShowPickaxeInfo(SetCurrentPickaxe(currentIndex)); //뭐지 이게. 분명 이게 아닐 것 같음.

        //왜냐하면, 클릭 한 이후에 바로 사라질 거거든.
        //그 값을 저장해두고 업데이트에서 ShowPickaxeInfo를 실행하는 것이 맞을 듯?

    }

    public void MoveRight() //오른쪽으로 움직일 때.
    {
        int clickLimit = PickaxeDataDB.GetPickaxeKeyCount();

        if (currentIndex >= clickLimit-1) return; //index가 6이면 null reference exception이 발생했다. 배열은 0부터 시작한다.

        currentIndex++;

        content.DOAnchorPosX(-currentIndex * moveDistance, moveDuration);

        //ShowPickaxeInfo(SetCurrentPickaxe(currentIndex)); 
    }

    //그러니까, currentIndex를 매개변수로 받아서,
    //PickaxeDataDB의 메서드를 실행하여,
    //키값에 맞는 곡괭이 SO를 반환한다는 건데.

    //뭔가 이상한데? 어떻게... SetCurrentPickaxe란 메서드가 굳이 필요한가? ShowPickaxeInfo에 합칠 수도 있을 텐데?
    private PickaxesDataSO SetCurrentPickaxe(int index) //currentIndex의 값에 따라 맞는 곡괭이 SO를 가져오는 메서드.
    {
        PickaxesDataSO currentPickaxe = PickaxeDataDB.GetStoneDataSO(index); //PickaxeDataDB의 정적 메서드를 실행하여
                                                                             //딕셔너리에서 키를 넣어 곡괭이 SO를 가져온다.

        return currentPickaxe; //가져온 곡괭이를 반환한다.
    }

    private void ShowPickaxeInfo(PickaxesDataSO currentPickaxe) //SetCurrentPickaxe를 통해 반환된 PickaxeDataSO형을 매개변수로 받게 됨
    {
        pickaxeInfo.DisplayNameText.text = currentPickaxe.DisplayName;
        pickaxeInfo.MiningPowerText.text = $"채굴 공격력 : {currentPickaxe.MiningPower}";
        pickaxeInfo.MiningSpeedText.text = $"채굴속도 : {currentPickaxe.MiningSpeed}";
        pickaxeInfo.MiningRadiusText.text = $"채굴반경 : {currentPickaxe.MiningRadius}";
        pickaxeInfo.CriticalChanceText.text = $"크리티컬 확률 : {currentPickaxe.CriticalChance}";
    }
}
