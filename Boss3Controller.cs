using UnityEngine;
using System.Collections;

public class Boss3Controller : MonoBehaviour {

	public GameObject[] formations;

	private int hitPoints = 400;
	private int pointValue = 150;
	private int damageSinceLastSpawn = 0;
	private bool dead = false;

	private GameObject current_formation;
	private int current_formation_id = 0;

	void Start () {
		//main boss object, drop in the first formation, and have it start populating with bits.
		SpawnBitFormation(current_formation_id);
	}

	void SpawnBitFormation (int whichFormation = 0) {

		current_formation = Instantiate (formations[whichFormation], transform.position, Quaternion.identity) as GameObject;
		current_formation.transform.SetParent (transform);
		current_formation.GetComponent<Boss3FormationController> ().SpawnBits ();
	}

	//will need methods to handle taking damage, telling the formation to start/stop spawning enemies,
	// becoming immune for a few seconds while swapping between formations, etc

	void OnTriggerEnter2D (Collider2D col) {
		//hit by projectile
		if (col.gameObject.GetComponent<Projectile> ()) {
			if (col.gameObject.GetComponent<Projectile> ().getDamageActive ()) {
				col.gameObject.GetComponent<Projectile> ().Destroy ();
				GameController.GetInstance ().ContinueCombo ();
				Damage (col.gameObject.GetComponent<Projectile> ().getDamageValue ());
			}
		}
	}

	void Damage (int amount) {
		hitPoints -= amount;

		if (hitPoints <= 0 && !dead) {
			GameController.GetInstance ().ScorePoints (pointValue);
			//Debug.Log ("Destroy boss!");
			dead = true;
			GameController.GetInstance ().QueueLevelEnd ();
			//Destroy (gameObject);
		} else if (!dead) {
			damageSinceLastSpawn += amount;
			if (damageSinceLastSpawn >= 50) {
				damageSinceLastSpawn = 0;
				if (current_formation_id == 0) {
					current_formation_id++;
					Destroy (current_formation);
					SpawnBitFormation (current_formation_id);
				}
				transform.GetComponentInChildren<Boss3FormationController> ().SpawnBits ();
			}
		}
	}
}
