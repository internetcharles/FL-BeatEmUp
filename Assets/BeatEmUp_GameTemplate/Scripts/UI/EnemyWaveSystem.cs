using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSystem : MonoBehaviour {

	[Header ("List of Enemy Waves")]
	public Transform positionMarkerLeft;
	public Level[] Levels;

	[SerializeField]
	public int currentWave;
	public int currentLevel;
	EnemyWave newWave;
	bool nextWave;
	public delegate void OnLevelEvent();
	public static event OnLevelEvent onLevelComplete;
	public static event OnLevelEvent onLevelStart;
	CameraFollow cam;


	void OnEnable(){
		Enemy.OnUnitDestroy += onUnitDestroy;
	}

	void OnDisable(){
		Enemy.OnUnitDestroy -= onUnitDestroy;
	}

	void Start(){
		currentWave = 0;
		currentLevel = 0;
		DisableEnemiesAtStart();
		CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
		if (cam != null) cam.SetLeftClampedPosition(positionMarkerLeft.position);
	}

	public void OnLevelStart(){
		if (onLevelStart != null) onLevelStart();
		if (cam != null) cam.SetLeftClampedPosition(positionMarkerLeft.position);
		StartWave();
	}

	void onUnitDestroy(GameObject g){
		if(Levels[currentLevel].EnemyWaves.Length > currentWave){
			newWave.RemoveEnemyFromWave();
			if(newWave.waveComplete()){
				currentWave += 1;
				if(!allWavesCompleted()){ 
					StartWave();
				} else{
					SpawnPowerups();
					currentWave = 0;
					currentLevel++;
//					if(onLevelComplete != null) onLevelComplete();
				}
			}
		}
	}

	public void StartWave(){
		newWave = Instantiate(Levels[currentLevel].EnemyWaves[currentWave]);
		if (Levels[currentLevel].EnemyWaves.Length > currentWave + 1)
        {
			nextWave = true;
		} else
        {
			nextWave = false;
        }
		CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
		if (cam != null){
			if(newWave.PositionMarker != null){
				Vector3 positionMarker = newWave.PositionMarker.position;
				//set camera X clamp position
				if (currentWave == 0){ 
					cam.SetNewClampPosition(positionMarker, 0f); //don't lerp at first start
				} else {
					cam.SetNewClampPosition(positionMarker, 1.5f);
				}
				float enemySpawnY = -1.12f;
				//enable the enemies of this wave
				foreach (GameObject g in newWave.EnemyList)
				{
					enemySpawnY += (newWave.EnemyList.Count / 4f);
					GameObject enemyFighter = Instantiate(g, new Vector3(positionMarker.x, positionMarker.y + enemySpawnY, positionMarker.z), Quaternion.identity);
					newWave.AddToLivingEnemies(g);
					enemyFighter.SetActive(true);
				}

			} else {
				Debug.Log("no position marker found in this wave");
			}
		} else {
			Debug.Log("no camera Follow component found on " + Camera.main.gameObject.name);
		}
		Invoke("SetEnemyTactics", .1f);
	}

	void SetEnemyTactics(){
		EnemyManager.SetEnemyTactics();
	}

	void DisableEnemiesAtStart(){
		foreach(EnemyWave wave in Levels[currentLevel].EnemyWaves)
		{
			foreach(GameObject g in wave.EnemyList){
				g.SetActive(false);
			}
		}
	}

	void SpawnPowerups()
    {
		CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
		GameObject player1 = GameObject.Find("Player1");
		for (int i = 0; i < Levels[currentLevel].levelPowerUps.Length; i++)
        {
			Vector3 itemSpawnLocation = new Vector3(player1.transform.position.x + ((i * 2) + 8), 1f, player1.transform.position.z);
			var powerUpTrack = Levels[currentLevel].levelPowerUps[i];
			var item = Instantiate(powerUpTrack.prefab, itemSpawnLocation, Quaternion.identity).GetComponent<PowerUpItem>();
			item.Init(powerUpTrack.icon, powerUpTrack.weapon, powerUpTrack.weaponStats);
		}
		cam.SetNewClampPosition(new Vector2(player1.transform.position.x + 19, -1.12f), 1.5f);
	}

	//returns true if all the waves are completed
	bool allWavesCompleted(){
		if(nextWave)
			return false;
		else 
			return true;
	}
}
