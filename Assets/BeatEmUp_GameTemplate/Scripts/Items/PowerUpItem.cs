using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpItem : MonoBehaviour
{
    public GameObject powerUpToEnable;
    public PowerUp powerUpStats;
    public SpriteRenderer iconRenderer;

    public delegate void OnLevelEvent();
    public static event OnLevelEvent onLevelComplete;

    public void Init(Sprite icon, GameObject powerUpToEnable, PowerUp powerUpStats)
    {
        iconRenderer.sprite = icon;
        this.powerUpToEnable = powerUpToEnable;
        this.powerUpStats = powerUpStats;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            if (powerUpToEnable != null)
            {
                if (powerUpStats.ability)
                {
                    powerUpToEnable.gameObject.SetActive(true);
                    PlayerInfo.instance.AddAbility(powerUpToEnable);
                }
                else
                {
                    powerUpToEnable.gameObject.SetActive(true);
                    Instantiate(powerUpToEnable, collision.gameObject.transform);
                    PlayerInfo.instance.AddPowerUp(powerUpToEnable);
                }
                if (onLevelComplete != null) onLevelComplete();
            }

            foreach (PowerUpItem power in FindObjectsOfType<PowerUpItem>())
            {
                if (power != this)
                    Destroy(power.gameObject);
            }

            Destroy(gameObject);
            
        }
    }
}
