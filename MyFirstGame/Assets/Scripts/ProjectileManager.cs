using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private TowerManager parent;
    private MonsterManager target;
    private Animator animator;
    private ProjectileTypeEnumes projectileType;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFollowTarget();
    }
    public void Init(TowerManager _parent)
    {
        parent = _parent;
        target = _parent.TargetMonster;
        projectileType = _parent.projectileType;
    }
    public void MoveFollowTarget()
    {
        if (target != null && target.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.AttackSpeed);
            Vector2 distance = target.transform.position - transform.position;
            float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(degree, Vector3.forward);
        }
        else if (target != null && !target.isActiveAndEnabled)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void ApplyDebuff()
    {
        if (!target.ProjectileType.Equals(projectileType))
        {
            float roll = Random.Range(0, 100);
            if (roll <= parent.Proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Crime")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage, projectileType);
                animator.SetTrigger("impact");
                ApplyDebuff();
            }
        }
    }
}
