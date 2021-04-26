using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Stat healthStat;
    [SerializeField]
    private ProjectileTypeEnumes projectileType;
    [SerializeField]
    private int invulnerability;
    private Vector3 destination;
    private Stack<Node> path;
    private float progress = 0;
    private Animator animator;
    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> removeDebuffs = new List<Debuff>();
    private List<Debuff> addDebuffs = new List<Debuff>();
    public bool IsDying { get { return healthStat.CurrentHealth <= 0; } }

    public ProjectileTypeEnumes ProjectileType { get => projectileType; }
    public float Speed { get => speed; set => speed = value; }

    public PointComponent GridPosition;
    public float MaxSpeed;
    private void Awake()
    {
        healthStat.Init();
        MaxSpeed = speed;
    }
    private void Update()
    {
        HandleDebuff();
        Move();
    }
    public void Spawn(float maxHealth)
    {
        this.healthStat.Refresh();
        this.healthStat.MaxHealth = maxHealth;
        this.healthStat.CurrentHealth = this.healthStat.MaxHealth;
        this.transform.position = GrassManager.Instance.BluePortalManager.transform.position;
        animator = GetComponent<Animator>();
        StartCoroutine(Scale(new Vector3(0, 0), new Vector3(1, 1)));
        SetPath(GrassManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool isRemove = false)
    {
        progress = 0;
        while (progress < 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
        if (isRemove)
        {
            Release();
        }
    }

    public void Move()
    {
        if (progress >= 1 && !IsDying)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPoint);
                    GridPosition = path.Peek().GridPoint;
                    destination = path.Pop().CenterPoint;
                }
            }
        }
    }

    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;
            Animate(GridPosition, path.Peek().GridPoint);
            GridPosition = path.Peek().GridPoint;
            destination = path.Pop().CenterPoint;
        }
    }

    private void Animate(PointComponent currentP, PointComponent nextP)
    {
        if (currentP.Y > nextP.Y)
        {
            //Move up
            animator.SetInteger("X", 0);
            animator.SetInteger("Y", -1);
        }
        else if (currentP.Y < nextP.Y)
        {
            //Move down
            animator.SetInteger("X", 0);
            animator.SetInteger("Y", 1);
        }
        else
        {
            if (currentP.X > nextP.X)
            {
                //Move left
                animator.SetInteger("X", -1);
                animator.SetInteger("Y", 0);
            }
            else
            {
                //Move right
                animator.SetInteger("X", 1);
                animator.SetInteger("Y", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D monsterCollider)
    {
        if (monsterCollider.tag == "Destination")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0, 0), true));
            GameManager.Instance.Lives--;
        }
    }

    public void Release()
    {
        GridPosition = GrassManager.Instance.StartPoint;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);
        addDebuffs.Clear();
        removeDebuffs.Clear();
        debuffs.Clear();
    }

    public void TakeDamage(float damage, ProjectileTypeEnumes _projectileType)
    {
        if (isActiveAndEnabled)
        {
            if (projectileType.Equals(_projectileType))
            {
                damage = damage / invulnerability;
            }
            SoundManager.Instance.PlaySFX("Splat");
            healthStat.CurrentHealth -= damage;
            if (healthStat.CurrentHealth <= 0)
            {
                GameManager.Instance.Currency += 2;
                animator.SetTrigger("die");
                GetComponent<SpriteRenderer>().sortingOrder--;
            }
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Any(x => x.GetType().Equals(debuff.GetType())))
        {
            addDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        removeDebuffs.Remove(debuff);
    }

    private void HandleDebuff()
    {
        if (addDebuffs.Count > 0)
        {
            debuffs.AddRange(addDebuffs);
            addDebuffs.Clear();
        }
        foreach (var debuff in removeDebuffs)
        {
            debuffs.Remove(debuff);
        }
        removeDebuffs.Clear();
        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }

    }
}
