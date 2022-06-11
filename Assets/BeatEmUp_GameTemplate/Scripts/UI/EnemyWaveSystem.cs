using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class EnemyWaveSystem : MonoBehaviour {

	[Header ("List of Enemy Waves")]
	public Transform positionMarkerLeft;
	public Level[] Levels;

	public Transform topOfLevel;
	public Transform bottomOfLevel;

	[SerializeField]
	public int currentWave;
	public int currentLevel;
	EnemyWave newWave;
	bool nextWave;
	public delegate void OnLevelEvent();
	public static event OnLevelEvent onLevelComplete;
	public static event OnLevelEvent onLevelStart;
	CameraFollow cam;

	[Header("UI")]
	public GameObject itemUI;
	public Image itemImage1;
	public Image itemImage2;
	public Image itemImage3;
	public TMP_Text itemText1;
	public TMP_Text itemText2;
	public TMP_Text itemText3;
	public Button itemButton1;
	public Button itemButton2;
	public Button itemButton3;



	void OnEnable(){
		Enemy.OnUnitDestroy += onUnitDestroy;
	}

	void OnDisable(){
		Enemy.OnUnitDestroy -= onUnitDestroy;
	}

	void Start(){
		itemButton1.onClick.AddListener(() => AddItemToPlayer(0));
		itemButton2.onClick.AddListener(() => AddItemToPlayer(1));
		itemButton3.onClick.AddListener(() => AddItemToPlayer(2));
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
				//enable the enemies of this wave
				foreach (GameObject g in newWave.EnemyList)
				{
					float spawnXOffset = Random.Range(0f, 3f);
					float enemySpawnY = Random.Range(0f, 5.5f);
					GameObject enemyFighter = Instantiate(g, new Vector3(positionMarker.x + spawnXOffset, Mathf.Clamp((bottomOfLevel.position.y + enemySpawnY), bottomOfLevel.position.y, topOfLevel.position.y), positionMarker.z), Quaternion.identity);
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
		// Delay by 5 seconds
		Levels[currentLevel].levelPowerUps = Levels[currentLevel].levelPowerUps.OrderBy(a => Guid.NewGuid()).ToList();
		itemUI.SetActive(true);

		for (int i = 0; i < 3; i++)
        {
			var currentItem = Levels[currentLevel].levelPowerUps[i];
			if (i == 0)
            {
				itemImage1.sprite = currentItem.icon;
				itemText1.text = currentItem.weaponStats.itemName;
            }
			if (i == 1)
            {
				itemImage2.sprite = currentItem.icon;
				itemText2.text = currentItem.weaponStats.itemName;
			}
			if (i == 2)
			{
				itemImage3.sprite = currentItem.icon;
				itemText3.text = currentItem.weaponStats.itemName;
			}
		}
	}

	void AddItemToPlayer(int itemIndex)
    {
		var powerUpToEnable = Levels[currentLevel].levelPowerUps[itemIndex];
		var powerUpStats = powerUpToEnable.weaponStats;
		if (powerUpStats.ability)
		{
			PlayerInfo.instance.AddAbility(powerUpStats.itemName);
		}
		else
		{
			PlayerInfo.instance.AddPowerUp(powerUpToEnable.weapon);
		}
		currentLevel++;
		itemUI.gameObject.SetActive(false);
		if (onLevelComplete != null) onLevelComplete();
	}

	//returns true if all the waves are completed
	bool allWavesCompleted(){
		if(nextWave)
			return false;
		else 
			return true;
	}
}
