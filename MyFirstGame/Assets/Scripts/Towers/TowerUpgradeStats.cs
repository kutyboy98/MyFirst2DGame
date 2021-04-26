using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeStats
{
    public int Price { get; private set; }
    public int Damage { get; private set; }
    public float DebuffDuration { get; private set; }
    public float ProcChance { get; private set; }
    public float SlowingFactor { get; private set; }
    public float TickTime { get; private set; }
    public int SpecialDamage { get; private set; }
    public TowerUpgradeStats(int price, int damage, float debuff, float proc, float tickTime, int specialDamage)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuff;
        this.ProcChance = proc;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
    }

    public TowerUpgradeStats(int price, int damage, float debuff, float proc, float slowingFactor)
    {
        this.Price = price;
        this.Damage = damage;
        this.DebuffDuration = debuff;
        this.ProcChance = proc;
        this.SlowingFactor = slowingFactor;        
    }
}
