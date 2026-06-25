using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoneDataSO", menuName = "Game/StoneDataSO")]
public class StoneDataSO : ScriptableObject
{ 
    [SerializeField] private string id;
    [SerializeField] private int tier;
    [SerializeField] private float maxHealth;
    [SerializeField] private StoneDataDB.StoneType stoneType;
    [SerializeField] private OreType oreType;
     
    [SerializeField] private List<OreAmount> rewardList = new(); 
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite stoneCrackSprite;

    public string Id => id;
    public int Tier => tier;
    public float MaxHealth => maxHealth;
    public StoneDataDB.StoneType StoneType => stoneType;
    public OreType OreType => oreType;
    public IReadOnlyList<OreAmount> RewardList => rewardList;
    public Sprite StoneSprite => stoneSprite;
    public Sprite StoneCrackSprite => stoneCrackSprite;
}