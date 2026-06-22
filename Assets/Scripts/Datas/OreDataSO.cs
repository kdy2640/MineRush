using UnityEngine;

[CreateAssetMenu(fileName = "OreDataSO", menuName = "Game/OreDataSO")]
public class OreDataSO : ScriptableObject
{
    [SerializeField] private OreType oreType;
    [SerializeField] private Color mainColor;
    [SerializeField] private Sprite oreSprite;
    [Header("Text + TMP 용 변수")]
    [SerializeField] private string displayName;
    [SerializeField,Tooltip("tmp_asset안에 있는 스프라이트 이름만 입력. ex)CopperOre")]
    private string tmpIconTag;
    

    public OreType OreType => oreType;
    public Color MainColor => mainColor;
    public Sprite OreSprite => oreSprite;
    public string DisplayName => displayName;
    public string TmpIconTag => tmpIconTag;
}
