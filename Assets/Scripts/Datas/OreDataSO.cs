using UnityEngine;

[CreateAssetMenu(fileName = "OreDataSO", menuName = "Game/OreDataSO")]
public class OreDataSO : ScriptableObject
{
    [SerializeField] private OreType oreType;
    [SerializeField] private Color mainColor;
    [SerializeField] private Sprite oreSprite;

    public OreType OreType => oreType;
    public Color MainColor => mainColor;
    public Sprite OreSprite => oreSprite;

}
