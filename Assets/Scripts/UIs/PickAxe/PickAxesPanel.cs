using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//0624 홍창성

public class PickAxesPanel : MonoBehaviour
{
    //Inspector 상에서 content를 할당한다.
    //아마도, 이 녀석을 이용해서 안에 생성하는 것도 되지 않을까. 
    [SerializeField] private RectTransform content;

    [SerializeField] private float moveDistance = 200f;
    [SerializeField] float moveDuration = 0.3f;

    [SerializeField] private CurrentPickaxeInfo pickaxeInfo;

    [SerializeField]private Image currentPickaxeImage; //여기에 image 프리팹을 넣는 거지.

    private int currentIndex = 0; //0에 가까울수록 Left버튼을 많이 누른 상태. 클수록 Right버튼을 많이 누른 상태


    private void Start()
    {
        InitializePanel();
    }


    private void Update()
    {
        pickaxeInfo.ShowPickaxeInfo(SetCurrentPickaxe(currentIndex));

        //ShowPickaxeInfo(SetCurrentPickaxe(currentIndex)); //0으로 초기화를 해뒀기에 처음에는 기본 곡괭이가 나온다.
        //근데, 메서드 안에 메서드가 있는 거 기묘한데 뭔가 아닌 것 같기도 하고.
    }

    public void MoveLeft() //왼쪽으로 움직일 때. Button의 Onclick에 등록하여 사용.
    {
        if (currentIndex <= 0) return;

        currentIndex--; //움직임 인덱스에서 1 감소

        content.DOAnchorPosX(-currentIndex * moveDistance, moveDuration);
        //현재 보여줘야 할 위치를 계산한다. 둘 다 -인 것과는 관계가 없다고 함.

        //ShowPickaxeInfo(SetCurrentPickaxe(currentIndex)); //뭐지 이게. 분명 여기서 쓰는 게 아닐 것 같음.

        //왜냐하면, 클릭 한 이후에 바로 사라질 거거든.
        //그 값을 저장해두고 업데이트에서 ShowPickaxeInfo를 실행하는 것이 맞을 듯?

    }

    public void MoveRight() //오른쪽으로 움직일 때.
    {
        int clickLimit = PickaxeDataDB.GetPickaxeKeyCount();

        if (currentIndex >= clickLimit-1) return; //index가 6이면 null reference exception이 발생했다. 배열은 0부터 시작한다.
                                                           //음 근데, 굳이 -1할 필요 없이 >로 처리하면 됐던 거 아닌가?

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



    //content 안에 현재 있는 곡괭이들의 이미지를 전부 일정 간격으로 생성하는 메서드
    //그러니까 위의 정보 출력과는 다르게, 얘는 업데이트에서 하는 게 아니라 Start나 Awake에서 해도 되잖아?
    private void InitializePanel() //음... 이 녀석과 위의 ShowPickaxeInfo는 사실 독립적이다. 어떻게 보면 꼼수.
    {
        for (int i = 0; i < PickaxeDataDB.GetPickaxeKeyCount(); i++) 
        {
            PickaxesDataSO currentPickaxe = SetCurrentPickaxe(i); //for문을 돌 때마다 얘가 바뀔 거 아냐?

            //아예 이 패널용 image 프리팹을 만들어두고 Instantiate로 생성한다.

            //그 다음, 그 패널 프리팹의 Source image를 찾아온 순서에 맞춰서 바꿔버리면 되는 거 아냐?

            Image image = Instantiate(currentPickaxeImage, content); //요렇게 생성하면                                                                                   
                                                                                       //애초에 Instantiate로 생성할 때 content 안의 자식으로 생성해버림.
                                                                                       //SetParent니 이런 거 신경 안 썼어도 됐음.

            image.sprite = currentPickaxe.Icon; //스프라이트 대입하기 위해서 위에서 생성 후 따로 변수 만들어서 담는 것.

            image.transform.localPosition = new Vector3(0 + i * 200, 0, 0);//여기에서 x축만 i * 200을 더해 간격 조정
                                                                                        //Horizontal Layout Group을 쓰는 방법도 있음
                                                                                       //이거 200을 그냥 moveDistance 쓰면 안 되나?


            //추가로,
            //구매하지 않은 곡괭이는 가림 표시 같은 거 해둬야 하고
            //구매하는 로직이 필요하다.
            //구매하는 로직이 구현됐다면, refresh하는 것도 필요하지 않을까.

            //음... currentIndex에 따라 어쨌든 곡괭이 정보가 바뀌잖아?
            //만약 그 곡괭이가 구매된 거면 return 때리고,
            //아니라면 TryBuy 같은 거 만들어서 하면 되려나...
        }
    }
}
