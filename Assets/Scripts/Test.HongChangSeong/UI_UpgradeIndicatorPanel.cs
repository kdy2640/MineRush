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

public class UI_UpgradeIndicatorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;

    //[SerializeField] private UpgradeData upgradeData;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //챗gpt가 알려준 버전은 메서드가 총 3개 있다.
    //1. SetData : SO Assest에서 이름이랑 레벨 정보를 뽑아와서 텍스트 칸에 전달.
    //2. SetCostText : SetData 내부에서 실행되는 private 메서드.
    //data에서 GetCosts를 통해 리스트 안에 있는 enum값이랑(종류), 숫자(가격)을 따로 뽑는다. 새로 리스트를 만들어줘야 한다.

    //광물타입과 필요한 광물값이 GetCosts 메서드를 통해 반환된다.

    //

    public void SetData(UpgradeData data) //UpgradeData SO 에셋을 매개변수로 받게 한다?
    {
        upgradeNameText.text = data.displayName; //데이터 안에 들어있는 displayName을 전달받아 텍스트를 바꾼다.


        //광물종류가 2개라면 여기서 for문을 돌리는데, 기존 텍스트를 띄우고 아랫칸에 쓰게 하던가
        //아예 게임 오브젝트를 하나 새로 생성해서 거기에다가 적던가 할 것인데, 이건 내가 할 수 있는 영역은 아닌 것 같음.




    }


}
