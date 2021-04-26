using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTowerManager : TowerManager
{
    [SerializeField]
    private float slowingFactor;

    public float SlowingFactor { get => slowingFactor; }
    protected override void Start()
    {
        TowerUpgrades = new TowerUpgradeStats[]
        {
            new TowerUpgradeStats(2,1,1,5,1),
            new TowerUpgradeStats(5,3,1,5,1)
        };
        base.Start();
    }
    public override Debuff GetDebuff()
    {
        return new FrostDebuff(TargetMonster, slowingFactor, DebuffDuration);
    }
    public override string GetStats()
    {
        if (NextUpgradeStats != null)
        {
            return $"<color=#ffa500ff><size=20><b>Ice</b></size></color>\nLevel: {Level}<color=#0A7100FF> +1</color>\nDamage: {Damage}<color=#0A7100FF> +{NextUpgradeStats.Damage}</color>\nDuration: {DebuffDuration}<color=#0A7100FF> +{NextUpgradeStats.DebuffDuration}</color>\nProc: {Proc}<color=#0A7100FF> +{NextUpgradeStats.ProcChance}</color>\nSpeed: {AttackSpeed}\nSlowing Factor: {slowingFactor}<color=#0A7100FF> +{NextUpgradeStats.SlowingFactor}</color>";
        }
        return $"<color=#ffa500ff><size=20><b>Ice</b></size></color>\n{base.GetStats()}\nSlowing Factor: {slowingFactor}";
    }

    public override void Upgrade()
    {
        this.slowingFactor += NextUpgradeStats.SlowingFactor;
        base.Upgrade();
    }
}
