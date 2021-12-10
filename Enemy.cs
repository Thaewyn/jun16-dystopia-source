using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int hp;
	public int pointValue;

	private float projectile_velocity = 10f;
	private float shotsPerSecond = 0.5f;

	private float timeSinceSpawn = 0f;
	private float onScreenDuration = 3f; //seconds between spawn and finishing the path
	private Vector2 posA;
	private Vector2 posB;
	private Vector2 posC;

	private bool destroyOnPathEnd = true;

	public void SetPath (int whichPath) {
		switch (whichPath) {
		case 0: // front mid to bottom mid arc
			posA = new Vector2 (9, 2);
			posB = new Vector2 (5, 7);
			posC = new Vector2 (4, -1);
			break;
		case 1: //front mid to top mid arc
			posA = new Vector2 (9, 4);
			posB = new Vector2 (5, -1);
			posC = new Vector2 (4, 7);
			break;
		case 2: //front high to back low
			posA = new Vector2 (9, 6);
			posB = new Vector2 (4, 3);
			posC = new Vector2 (-1, 2);
			break;
		case 3: //back high to bottom mid
			posA = new Vector2 (-1, 6);
			posB = new Vector2 (6, 6);
			posC = new Vector2 (6, -1);
			break;
		case 4://back low to front high
			posA = new Vector2 (-1, -1);
			posB = new Vector2 (6, 6);
			posC = new Vector2 (8, 8);
			onScreenDuration = 4f;
			break;
		case 5: //front high to back high
			posA = new Vector2 (9, 6);
			posB = new Vector2 (4, 3);
			posC = new Vector2 (-1, 5);
			break;
		case 6: //front low to back low
			posA = new Vector2 (9, 0);
			posB = new Vector2 (4, 3);
			posC = new Vector2 (-1, 1);
			break;
		case 7://back high to front low
			posA = new Vector2 (-1, 7);
			posB = new Vector2 (6, 6);
			posC = new Vector2 (8, -1);
			break;
		case 8://mid low to back high (train)
			posA = new Vector2 (5, -1);
			posB = new Vector2 (6, 6);
			posC = new Vector2 (-1, 7);
			break;
		case 10: //front high to stop mid-high
			posA = new Vector2 (9, 7);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 5);
			destroyOnPathEnd = false;
			break;
		case 11: //front high to stop mid
			posA = new Vector2 (9, 7);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 3);
			destroyOnPathEnd = false;
			break;
		case 12: //front high to stop low
			posA = new Vector2 (9, 7);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 1);
			destroyOnPathEnd = false;
			break;
		case 13: //front low to stop mid-high
			posA = new Vector2 (9, -1);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 5);
			destroyOnPathEnd = false;
			break;
		case 14: //front low to stop mid
			posA = new Vector2 (9, -1);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 3);
			destroyOnPathEnd = false;
			break;
		case 15: //front low to stop low
			posA = new Vector2 (9, -1);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 1);
			destroyOnPathEnd = false;
			break;
		case 16: //back to stop high
			posA = new Vector2 (-1, 6);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 5);
			destroyOnPathEnd = false;
			break;
		case 17: //back to stop low
			posA = new Vector2 (-1, -1);
			posB = new Vector2 (6, 3);
			posC = new Vector2 (6, 1);
			destroyOnPathEnd = false;
			break;
		default: 
			//Debug.Log ("Default path");
			posA = new Vector2 (9, 2);
			posB = new Vector2 (5, 7);
			posC = new Vector2 (4, -1);
			break;
		}
	}

	void Update () {

		HandleShooting ();

		HandlePosition ();

		HandleGarbageCleanup ();
	}

	void HandleShooting () {

		float probability = Time.deltaTime * shotsPerSecond;

		if (Random.value < probability) { //sometimes a very long frame can mean that probability is greater than one, but that just means this value will definitely be true
			Fire ();
		}
	}

	void Fire() {
		//Vector3 startPos = transform.position + new Vector3 (-1f, 0f, 0f);
		//GameObject bullet = Instantiate (projectilePrefab, transform.position, Quaternion.identity) as GameObject;
		GameObject bullet = ProjectileContainer.GetInstance().nextAvailableProjectile();
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;
		bullet.SetActive (true);
		Vector3 bulletVelocity = new Vector3 (-projectile_velocity, 0, 0);
		bullet.GetComponent<Rigidbody2D> ().velocity = bulletVelocity;
		bullet.transform.SetParent (ProjectileContainer.GetInstance ().transform);
	}
		
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
		hp -= amount;
		if (hp <= 0) {
			//dead/
			GameController.GetInstance().ScorePoints(pointValue);
			Destroy (gameObject);
		} else {
			//animate damage?
		}
	}

	void HandlePosition () {
		//this should handle this enemy's position on screen, based on the time since they spawned (compared against the bezier function below).
		timeSinceSpawn += Time.deltaTime;
		transform.position = Bezier (timeSinceSpawn / onScreenDuration, posA, posB, posC);
	}

	void HandleGarbageCleanup () {
		if (timeSinceSpawn >= onScreenDuration && destroyOnPathEnd) {
			Destroy (gameObject);
		}
	}

	Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c) {
		var ab = Vector2.Lerp(a,b,t);
		var bc = Vector2.Lerp(b,c,t);
		return Vector2.Lerp(ab,bc,t);
	}
}
