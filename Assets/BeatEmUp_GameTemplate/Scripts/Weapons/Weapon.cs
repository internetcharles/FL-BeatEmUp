using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public PowerUp weaponStats;
    public Projectile projectile;
    public KeyCode fireKey = KeyCode.Mouse0;
    private PlayerMovement playerM;

    private void Start()
    {
        playerM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            GameObject p = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
            Projectile pScript = p.GetComponent<Projectile>();
            DIRECTION direction = playerM.getCurrentDirection();
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
        }
    }
}
