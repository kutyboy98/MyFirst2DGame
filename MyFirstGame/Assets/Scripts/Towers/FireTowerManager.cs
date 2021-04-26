using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowerManager : TowerManager
{
    [SerializeField]
    private float tickTime;
    [SerializeField]
    private float tickDamage;

    public float TickTime { get => tickTime; }
    public float TickDamage { get => tickDamage; }

    protected override void Start()
    {
        TowerUpgrades = new TowerUpgradeStats[]
        {
            new TowerUpgradeStats(2,1,1,5,-0.1f,1),
            new TowerUpgradeStats(5,3,1,5,-0.1f,1)
        };
        base.Start();
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(TargetMonster, tickTime, tickDamage, DebuffDuration);
    }

    public override string GetStats()
    {
        if ((NextUpgradeStats?.Damage ?? null) != null)
        {
            return $"<color=#ffa500ff><size=20><b>Fire</b></size></color>\nLevel: {Level}<color=#0A7100FF> +1</color>\nDamage: {Damage}<color=#0A7100FF> +{NextUpgradeStats.Damage}</color>\nDuration: {DebuffDuration}<color=#0A7100FF> +{NextUpgradeStats.DebuffDuration}</color>\nProc: {Proc}<color=#0A7100FF> +{NextUpgradeStats.ProcChance}</color>\nSpeed: {AttackSpeed}\nTick Time: {tickTime}<color=#AA1000ff> {NextUpgradeStats.TickTime}</color>\nTick Damage: {tickDamage}<color=#0A7100FF> +{NextUpgradeStats.SpecialDamage}</color>";
        }
        return $"<color=#ffa500ff><size=20><b>Fire</b></size></color>\n{base.GetStats()}\nTick Time: {tickTime}\nTick Damage: {tickDamage}";
    }

    public override void Upgrade()
    {
        this.tickTime += NextUpgradeStats.TickTime;
        this.tickDamage += NextUpgradeStats.SpecialDamage;
        base.Upgrade();
    }
}
