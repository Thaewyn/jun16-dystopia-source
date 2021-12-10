using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileContainer : MonoBehaviour {

	private static ProjectileContainer instance = null;

	public GameObject playerProjectilePrefab;
	public GameObject enemyProjectilePrefab;
	public GameObject bigEnemyProjectilePrefab;

	private int playerProjectilePool = 100;
	private int enemyProjectilePool = 30;
	private int bigEnemyProjectilePool = 50;

	List<GameObject> playerProjectiles;
	List<GameObject> enemyProjectiles;
	List<GameObject> bigEnemyProjectiles;

	void Start () {
		
		//set up player projectile pool
		playerProjectiles = new List<GameObject> ();
		for (int i = 0; i < playerProjectilePool; i++) {
			GameObject obj = (GameObject)Instantiate (playerProjectilePrefab);
			obj.SetActive (false);
			obj.transform.SetParent (transform);
			playerProjectiles.Add (obj);
		}

		float red = 1f;
		float green = 1f;
		float blue = 1f;

		//enemy projectile color for this level
		switch (GameController.GetInstance ().GetCurrentLevel ()) {
		case 1:
			red = 0.43f;
			green = 0.93f;
			blue = 0.45f;
			break;
		case 2:
			red = 0f;
			green = 0f;
			blue = 1f;
			break;
		case 3:
			red = 1f;
			green = 0f;
			blue = 0f;
			break;
		}

		//set up enemy projectile pool
		enemyProjectiles = new List<GameObject> ();
		for (int i = 0; i < enemyProjectilePool; i++) {
			GameObject obj = (GameObject)Instantiate (enemyProjectilePrefab);
			obj.SetActive (false);
			obj.transform.SetParent (transform);
			obj.GetComponent<Projectile> ().setColor (red, green, blue, 1f);
			enemyProjectiles.Add (obj);
		}
		bigEnemyProjectiles = new List<GameObject> ();
		for (int i = 0; i < bigEnemyProjectilePool; i++) {
			GameObject obj = (GameObject)Instantiate (bigEnemyProjectilePrefab);
			obj.SetActive (false);
			obj.transform.SetParent (transform);
			obj.GetComponent<Projectile> ().setColor (red, green, blue, 1f);
			bigEnemyProjectiles.Add (obj);
		}
	}

	public GameObject nextAvailableProjectile(bool forPlayer = false) {
		if (forPlayer) {
			for (int i = 0; i < playerProjectiles.Count; i++) {
				if (!playerProjectiles [i].activeInHierarchy) {
					return playerProjectiles [i];
				}
			}
			//nothing available! Create a new one and push it into the List.
		} else {
			for (int i = 0; i < enemyProjectiles.Count; i++) {
				if (!enemyProjectiles [i].activeInHierarchy) {
					return enemyProjectiles [i];
				}
			}
			//nothing available! Create a new one and push it into the List.
		}
		return null;
	}
	public GameObject nextAvailableBigProjectile(bool forPlayer = false) {
		for (int i = 0; i < bigEnemyProjectiles.Count; i++) {
			if (!bigEnemyProjectiles [i].activeInHierarchy) {
				return bigEnemyProjectiles [i];
			}
		}
		//nothing available! Create a new one and push it into the List.
		return null;
	}

	public static ProjectileContainer GetInstance() {
		return instance;
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
}
