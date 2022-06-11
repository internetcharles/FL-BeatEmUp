using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public PowerUp weaponStats;
    public Projectile projectile;
    public KeyCode fireKey;
    private PlayerMovement playerM;
    DIRECTION direction;
    private bool allowFire;

    private void Start()
    {
        playerM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        allowFire = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(fireKey) && allowFire)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        // Fix later
        allowFire = false;
        playerM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        GameObject p = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
        Projectile pScript = p.GetComponent<Projectile>();
        direction = playerM.getCurrentDirection();
        Debug.Log(direction);
        if (direction == DIRECTION.Right)
        {
            pScript.FireInDirection(Vector3.right);
        }
        if (direction == DIRECTION.Down)
        {
            pScript.FireInDirection(Vector3.down);
        }
        if (direction == DIRECTION.Left)
        {
            pScript.FireInDirection(Vector3.left);
        }
        if (direction == DIRECTION.Down)
        {
            pScript.FireInDirection(Vector3.down);
        }
        yield return new WaitForSeconds(weaponStats.rateOfFire);
        allowFire = true;
    }
}
