using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoneDataSO", menuName = "Game/StoneDataSO")]
public class StoneDataSO : ScriptableObject
{ 
    [SerializeField] private string id;
    [SerializeField] private int tier;
    [SerializeField] private float maxHealth;
     
    [SerializeField] private List<OreAmount> rewardList = new(); 
    [SerializeField] private GameObject solidStonePrefab;

    public string Id => id;
    public int Tier => tier;
    public float MaxHealth => maxHealth;
    public IReadOnlyList<OreAmount> RewardList => rewardList;
    public GameObject SolidStonePrefab => solidStonePrefab;
}