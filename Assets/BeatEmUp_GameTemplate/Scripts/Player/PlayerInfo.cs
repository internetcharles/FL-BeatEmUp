using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;
    public List<GameObject> playerPowerUps = new List<GameObject>();
    public List<GameObject> playerAbilities = new List<GameObject>();
    public List<string> acquiredItems = new List<string>();
    private GameObject player;

    [Header("Hotbar")]
    public GameObject hotbarUI;
    public GameObject[] powerupPanels;
    public Image[] powerupImages;
    public Image[] itemImages;

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
        hotbarUI.SetActive(true);
        for (int i = 0; i < playerAbilities.Count; i++)
        {
            var weapon = playerAbilities[i].GetComponent<PowerupStats>();
            powerupPanels[i].SetActive(true);
            powerupImages[i].sprite = weapon.stats.sprite;
            Instantiate(playerAbilities[i], player.transform);
        }
        for (int i = 0; i < playerPowerUps.Count; i++)
        {
            var weapon = playerPowerUps[i].GetComponent<PowerupStats>();
            itemImages[i].gameObject.SetActive(true);
            itemImages[i].sprite = weapon.stats.sprite;
            Instantiate(playerPowerUps[i], player.transform);
        }
    }

    public void AddAbility(GameObject ability)
    {
        playerAbilities.Add(ability);
    }

    public void AddItem(string item)
    {
        acquiredItems.Add(item);
    }
}
