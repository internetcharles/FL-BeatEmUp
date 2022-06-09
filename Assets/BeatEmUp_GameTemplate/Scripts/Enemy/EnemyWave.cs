using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWave : MonoBehaviour
{
	public string WaveName = "Wave";
	public Transform PositionMarker; // the screen stops scrolling at this position marker until the wave is complete
	public List<GameObject> EnemyList = new List<GameObject>();
	public List<GameObject> LivingEnemies = new List<GameObject>();

    public bool waveComplete()
	{
		if (LivingEnemies.Count == 0) {
			Debug.Log("Enemy count reached");
			Destroy(gameObject);
			return true;
		} else {
			return false;
		}
	}

	public void RemoveEnemyFromWave()
	{
		LivingEnemies.RemoveAt(0);
	}

	public void AddToLivingEnemies(GameObject g)
    {
		LivingEnemies.Add(g);
    }
}