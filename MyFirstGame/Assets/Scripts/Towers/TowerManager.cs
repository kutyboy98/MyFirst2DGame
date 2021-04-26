using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private MonsterManager targetMonster;
    private Queue<MonsterManager> monstersInRange;
    [SerializeField]
    public ProjectileTypeEnumes projectileType;
    [SerializeField]
    private float resetTime;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float debuffDuration;
    [SerializeField]
    private float proc;
    private float realTime = 0;
    private bool canAttack = true;

    public MonsterManager TargetMonster { get => targetMonster; }
    public float AttackSpeed { get => attackSpeed; }
    public int Damage { get => damage; }
    public float DebuffDuration { get => debuffDuration; }
    public float Proc { get => proc; }

    public int Price;

    public TowerUpgradeStats[] TowerUpgrades { get; protected set; }

    public int Level { get; set; }

    public TowerUpgradeStats NextUpgradeStats
    {
        get
        {
            if (TowerUpgrades.Length > (Level - 1))
            {
                return TowerUpgrades[Level - 1];
            }
            return null;
        }
    }

    public string PriceText
    {
        get
        {
            return $"<color=green>+{Price / 2}</color> <color=yellow>$</color>";
        }
    }

    public string UpgradeText
    {
        get
        {
            return $"<color=red>-{NextUpgradeStats?.Price ?? 0}</color> <color=yellow>$</color>";
        }
    }

    protected virtual void Start()
    {
        Level = 1;
        monstersInRange = new Queue<MonsterManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void Deselect()
    {
        spriteRenderer.enabled = false;
    }

    private void Attack()
    {
        if (!canAttack)
        {
            realTime += Time.deltaTime;
            if (realTime >= resetTime)
            {
                canAttack = true;
                realTime = 0;
            }
        }
        if ((targetMonster?.IsDying ?? true) && (monstersInRange?.Count ?? 0) > 0 && ((monstersInRange?.Peek() ?? default(MonsterManager)).isActiveAndEnabled))
        {
            targetMonster = monstersInRange.Dequeue();
        }
        if (targetMonster != null && targetMonster.isActiveAndEnabled)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
        if (targetMonster != null && !targetMonster.isActiveAndEnabled)
        {
            targetMonster = null;
        }
    }

    private void Shoot()
    {
        ProjectileManager projectile = GameManager.Instance.Pool.GetObject<ProjectileTypeEnumes>(projectileType).GetComponent<ProjectileManager>();
        projectile.transform.position = transform.position;
        projectile.Init(this);
    }

    private void OnTriggerEnter2D(Collider2D anything)
    {
        if (anything.tag.Equals("Crime"))
        {
            monstersInRange.Enqueue(anything.GetComponent<MonsterManager>());
        }
    }

    private void OnTriggerExit2D(Collider2D anything)
    {
        if (anything.tag.Equals("Crime"))
        {
            targetMonster = null;
        }
    }

    public virtual string GetStats()
    {
        return $"Level: {Level}\nDamage: {damage}\nDuration: {debuffDuration}\nProc: {proc}\nSpeed: {attackSpeed}";
    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgradeStats.Price;
        Price += NextUpgradeStats.Price;
        this.damage += NextUpgradeStats.Damage;
        this.debuffDuration += NextUpgradeStats.DebuffDuration;
        this.proc += NextUpgradeStats.ProcChance;
        Level++;
        GameManager.Instance.UpgradeStats();
    }

    public abstract Debuff GetDebuff();
}
