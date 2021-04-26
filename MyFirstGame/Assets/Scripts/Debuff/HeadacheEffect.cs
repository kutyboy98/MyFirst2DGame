using UnityEngine;

public class HeadacheEffect : MonoBehaviour
{
    public float DamageEffect { get; set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Crime")
        {
            other.GetComponent<MonsterManager>().TakeDamage(DamageEffect, ProjectileTypeEnumes.HeadchacheProjectile);
            Destroy(gameObject);
        }
    }
}
