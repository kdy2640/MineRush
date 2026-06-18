using System.Text;
using TMPro;
using UnityEngine;


//홍창성
//0618
//UpgradeIndicatorPanel에 붙일 스크립트.
//UpgradeSOdata 에셋을 참조하여, 거기에 들어있는 정보들을 자신이 갖고있는 텍스트 박스들에 띄운다.
//그런데, 광물 종류와 수량이 2개 이상일 수도 있다.
//배열을 필드로 갖고, for문을 돌려서 그 수만큼 해야 하는 걸지. << 이건 나중에 생각하자.

//아마도, 마우스를 갔다댔을 때 해당 노드가 갖고 있는 UpgradeSO data 에셋을 받아오는 게 맞을 텐데.

//그리고, 이 패널이 스탯 업그레이드와 스킬 업그레이드 두 가지에 쓰여야 될 것 같은데,

//내부에서 if문을 걸어서, UprgaredSO data 에셋에 무언가가 있는가를 검사한다.

//없으면 없는 걸로 출력하고, 있으면 스킬이라는 뜻이므로 스킬 방식으로 출력하게 한다?

//0619 : 작업사항 기록용으로 남겨두고 일단 잠정보류. 프로퍼티 제공용 스크립트는 UpgradePanelIndicator 스크립트로 이전함.

//솔직히 근데, 굳이 프로퍼티로 했어야 했을까? 그냥 일반 필드로 했어도 됐을 것 같은데...

public class UI_UpgradeIndicatorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;

    //[SerializeField] private UpgradeData upgradeData;


    //챗gpt가 알려준 버전은 메서드가 총 3개 있다.
    //1. SetData : SO Assest에서 이름이랑 레벨 정보를 뽑아와서 텍스트 칸에 전달.
    //2. SetCostText : SetData 내부에서 실행되는 private 메서드.
    //data에서 GetCosts를 통해 리스트 안에 있는 enum값이랑(종류), 숫자(가격)을 따로 뽑는다. 새로 리스트를 만들어줘야 한다.

    //광물타입과 필요한 광물값이 GetCosts 메서드를 통해 반환된다.

    //

    public void SetData(UpgradeData data, int currentLevel, int maxLevel) //UpgradeData SO 에셋을 매개변수로 받게 한다?
    {
        //여기에서 그냥 if (data.skill == none) 이런 식으로 하면 스킬인지 아닌지 구분할 수 있을 것.

        upgradeNameText.text = data.displayName; //데이터 안에 들어있는 displayName을 전달받아 텍스트를 바꾼다.

        SetCostText(data, currentLevel); //광물 종류와 현재 레벨에 맞는 광물량 텍스트를 바꾼다.

        //설명 텍스트는 아마 프리팹에 있었던 것 같은데.

        upgradeLevelText.text = $"{currentLevel} / {maxLevel}"; //현재레벨과 최대 레벨을 바꾼다.


        //광물종류가 2개라면 여기서 for문을 돌리는데, 기존 텍스트를 띄우고 아랫칸에 쓰게 하던가
        //아예 게임 오브젝트를 하나 새로 생성해서 거기에다가 적던가 할 것인데, 이건 내가 할 수 있는 영역은 아닌 것 같음.


        //보니까... 스킬과 같은 경우에는 Description이 프리팹에 붙어있고, 그냥 스탯 업그레이드에는 Desciptrion이 따로 없음.
        //여기서 if else를 통한 하드코딩을 해서
        //스킬이 아닌 경우에는 안에 표기된 statModifier를 가져오는데, 
        //계산 방식은 필요없고, 스탯종류와 값만 가져오면 됨.



        //그니까, UpgradeNodePanelController에 

        //메서드를 넣는 게 아니라 여기 {}안에 있는 코드를 넣게 되는 느낌으로...

        //스킬인 경우에는 Description이 있으니까....
        //그냥 설명은 SO에 다시 고쳐보신다고 하심.

        //일단은... if(data.skill == none)이 맞았긴 한데, 조금 바뀌어야 될 상황이 생김.

    }

    private void SetCostText(UpgradeData data, int currentLevel)
    {
        //StringBuilder sb = new StringBuilder();

        ////var costs = data.GetCosts(currentLevel);

        //foreach (var cost in costs)
        //{
        //    sb.AppendLine($"{cost.oreType} : {cost.amount}"); //나중에는 아마도 string이 아니라 이미지 등으로 교체될 건데 이 부분은 어떡하지.

        //}
        ////현재 시점에선 필요한 자원과 필요 수량을 한 곳에 띄우지만,
        ////나중에는 필요한 자원은 이미지 영역 하나 만들어서 거기다 띄우고, 수량은 그대로 텍스트로 띄우고 이렇게 될 것 같은데.

        //upgradeCostText.text = sb.ToString();


        //데이터에 직접 접근해서 GetCosts를 쓰지말고
        //UpgradeState를 경유하여 GetCurrentCost를 해야하고,

        //거기에 넣을 현재 레벨은 UpgradeManager가 갖고 있음.
        //UpgradeState 하나가 노드 데이터 그 자체라고 보면 된다.

        //gpt에게 upgradeManager와 UpgradeState, UpgradeData, OreAmount 넣어서 분석 요청하기.

        //현재레벨을 불러오려면 게임매니저 - 인스턴스 - 업그레이드 - 그다음 GetState해서 level을 해야 '현재 레벨'을 가져온다.


    }

    //그냥 업그레이드의 경우에는, 레벨에 따라서 효과가 변동하지 않는다.
    //그러니 매개변수로 data만 받으면 될 것 같은데?
    private void SetDescriptionForNonSkill() //스킬이 아닌 거
    {
        //위와 대체로 비슷하나, statModifier에서 스탯종류와 값을 추출해야 한다.

        int temp = 1;

    }
}
