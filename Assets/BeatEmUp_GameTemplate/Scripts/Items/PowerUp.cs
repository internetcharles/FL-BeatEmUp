using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp", order = 1)]
public class PowerUp : ScriptableObject
{
    public string itemName;
    public bool ability;
    public Sprite sprite;
    public float damage;
    public bool projectile;
    public float projectileSpeed;
    public float projectileLifespan;
    public bool manualWeapon;
    public bool autoFire;
    public float homing;
    public float range;
    public float projectileCount;
    public float cooldown;
    public float aoeRadius;
    public float aoeLimit;
    public float rateOfFire;
    public AttackType attackType;
}
