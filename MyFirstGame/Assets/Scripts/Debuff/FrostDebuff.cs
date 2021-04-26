using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostDebuff : Debuff
{
    private float slowingFactor;
    private bool isApplied;
    public FrostDebuff(MonsterManager target, float slowingFactor, float duration) : base(target, duration)
    {
        this.slowingFactor = slowingFactor;
    }
    public override void Update()
    {
        if (target != null)
        {
            if (!isApplied)
            {
                isApplied = true;
                target.Speed -= (target.MaxSpeed * slowingFactor) / 100;
            }
        }
        base.Update();
    }

    public override void Remove()
    {
        if (target != null)
        {
            target.Speed = target.MaxSpeed;
        }
        base.Remove();
    }
}
