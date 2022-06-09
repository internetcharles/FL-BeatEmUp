using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public PowerUp weaponStats;
    public GameObject impactPrefab;
    public float damage => weaponStats.damage;
    public float speed => weaponStats.projectileSpeed;
    public float lifespan => weaponStats.projectileLifespan;
    public float aoeRadius => weaponStats.aoeRadius;
    public int aoeLimit => (int)weaponStats.aoeLimit;
    public bool heatSeeking => weaponStats.homing >= 1;
    GameObject target = null;
    Vector3 originalTargetPos = Vector3.zero;
    Vector3 originPos = Vector3.zero;
    float curLifetime = 0f;

    private void Update()
    {
        if (curLifetime > lifespan)
        {
            Destroy(gameObject);
            return;
        }

        if (target != null)
        {
            //NPCManager npc = target.GetComponent<NPCManager>();
            if (heatSeeking)
                transform.position = Vector2.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
            else
            {
                // transform.position = Vector2.Lerp(transform.position, originalTargetPos, Time.deltaTime * speed);
                transform.Translate((originalTargetPos - originPos).normalized * Time.deltaTime * speed);
            }

        }
        else if (weaponStats.manualWeapon)
        {
            transform.Translate((originalTargetPos - originPos).normalized * Time.deltaTime * speed);
        }
        else
        {
            Destroy(gameObject);
        }

        curLifetime += Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        originPos = transform.position;
        originalTargetPos = target.transform.position;
    }

    public void FireInDirection(Vector3 direction)
    {
        originalTargetPos = direction;
    }

    public void Collided(Enemy npc)
    {
        if (npc.CompareTag("Enemy"))
        {
            DamageObject d = new DamageObject(damage, gameObject);
            d.attackType = weaponStats.attackType;
            npc.GetComponent<EnemyAI>().Hit(d);
            npc.GetComponent<HealthSystem>().SubstractHealth(d.damage);
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        if (impactPrefab != null)
        {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
        }
    }
}
