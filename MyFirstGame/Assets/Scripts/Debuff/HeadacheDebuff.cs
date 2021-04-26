using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadacheDebuff : Debuff
{
    private float tickTime;
    private float tickDamage;
    private float timeSinceTick;
    private HeadacheEffect starPrefab;
    public HeadacheDebuff(MonsterManager target, float tickTime, float tickDamage, float duration, HeadacheEffect starPrefab) : base(target, duration)
    {
        this.tickTime = tickTime;
        this.tickDamage = tickDamage;
        this.starPrefab = starPrefab;
    }

    // Start is called before the first frame update
    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;
            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;                
                StarDrop();
            }
        }
        base.Update();
    }

    public void StarDrop()
    {
        HeadacheEffect tmp = GameObject.Instantiate(starPrefab, target.transform.position, Quaternion.identity);
        tmp.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        tmp.DamageEffect = tickDamage;
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
