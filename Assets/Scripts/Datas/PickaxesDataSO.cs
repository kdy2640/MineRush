
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PickaxeData")]
public class PickaxesDataSO : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private int tier;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private UpgradeData upgradeData;

    [Header("Stats")]
    [SerializeField] private int miningPower;
    [SerializeField] private float miningSpeed;
    [SerializeField] private float miningRadius;
    [SerializeField] private float criticalChance;

    

    public int Tier => tier;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public UpgradeData UpgradeData => upgradeData;

    
    public int MiningPower => miningPower;
    public float MiningSpeed => miningSpeed;
    public float MiningRadius => miningRadius;
    public float CriticalChance => criticalChance;
}