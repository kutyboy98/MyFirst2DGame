using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    private float tickTime;
    private float tickDamage;
    private float timeSinceTick;
    public FireDebuff(MonsterManager target, float tickTime, float tickDamage, float duration) : base(target, duration)
    {
        this.tickTime = tickTime;
        this.tickDamage = tickDamage;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                target.TakeDamage(tickDamage, ProjectileTypeEnumes.FireProjectile);
            }
        }
        base.Update();
    }
}
