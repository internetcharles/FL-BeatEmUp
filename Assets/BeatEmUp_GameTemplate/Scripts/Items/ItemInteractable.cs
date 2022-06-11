using UnityEngine;
using System.Collections;

public class ItemInteractable : ItemActions {

	void OnEnable(){
		ItemManager.AddItemToList(gameObject);
	}

	void OnDisable() {
		ItemManager.RemoveItemFromList(gameObject);
	}

	void Start(){
		SetSortingOrder();
	}

	//trigger enter
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.CompareTag ("Player") && item.isPickup){
			Debug.Log("Collision detected");
			coll.GetComponent<PlayerCombat>().itemInRange = gameObject;
		}
	}

	//trigger exit
	void OnTriggerExit2D(Collider2D coll){
		if(coll.CompareTag ("Player") && item.isPickup){ 
			coll.GetComponent<PlayerCombat>().itemInRange = null;
		}
	}

	//Set sorting order
	void SetSortingOrder() {
		SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
		if(sr != null) sr.sortingOrder = (int)(transform.position.y * -20);
	}

	//activates this item
	public void ActivateItem(GameObject _target) 
	{
		//set target
		target = _target;

		//activate this items function
		if(item.callMethod != "") Invoke(item.callMethod, 0);

		//play sfx
		GlobalAudioPlayer.PlaySFX(item.sfx);

		//add to list
		PlayerInfo.instance.AddItem(item.itemName);

		//remove item
		Destroy(gameObject);
	}
}