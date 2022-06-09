using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : MonoBehaviour {

	void OnEnable(){
		EnemyWaveSystem.onLevelStart += spawnPlayer;
	}

	void OnDisable(){
		EnemyWaveSystem.onLevelStart -= spawnPlayer;
	}

	void spawnPlayer() {
		Destroy(GameObject.Find("Player1"));
		GameObject player = Instantiate(Resources.Load("Player1"), transform.position, Quaternion.identity) as GameObject;
		player.name = "Player1";
		var cam = Camera.main.GetComponent<CameraFollow>();
		cam.followTarget = player;
		cam.SetLeftClampedPosition(GameObject.Find("EnemyWaveSystem").GetComponent<EnemyWaveSystem>().positionMarkerLeft.position);
		PlayerInfo.instance.InstantiatePowerUps(player);
	}
}
