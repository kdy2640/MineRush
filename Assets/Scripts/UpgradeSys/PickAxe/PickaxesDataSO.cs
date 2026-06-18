
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PickAxe Data")]
public class PickaxesDataSO : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private int tier;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;

    [Header("Stats")]
    [SerializeField] private int miningPower;
    [SerializeField] private float miningSpeed;
    [SerializeField] private float miningRadius;
    [SerializeField] private float criticalChance;

    [Header("Shop")]
    [SerializeField] private List<OreAmount> amounts;

    public int Tier => tier;
    public string DisplayName => displayName;
    public Sprite Icon => icon;

    public int MiningPower => miningPower;
    public float MiningSpeed => miningSpeed;
    public float MiningRadius => miningRadius;
    public float CriticalChance => criticalChance;
}