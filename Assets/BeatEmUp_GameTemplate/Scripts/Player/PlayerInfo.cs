using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;
    public List<GameObject> playerPowerUps = new List<GameObject>();
    private GameObject player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Machine singleton is already initialized");
            Destroy(gameObject);
        }
        else if (instance != this)
        {
            instance = this;
        }

        player = GameObject.Find("Player1");
    }

    public void AddPowerUp(GameObject powerUp)
    {
        playerPowerUps.Add(powerUp);
    }

    public void InstantiatePowerUps(GameObject player)
    {
        foreach (GameObject powerUp in playerPowerUps)
        {
            Instantiate(powerUp, player.transform);
        }
    }
}
