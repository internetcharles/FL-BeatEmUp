using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public EnemyWave[] EnemyWaves;
    public List<PowerUpTrack> levelPowerUps = new List<PowerUpTrack>();
    public GameObject[] spawnableItems;
} 
