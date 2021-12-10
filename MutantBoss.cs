using UnityEngine;
using System.Collections;

public class MutantBoss : MonoBehaviour {

	private int hitPoints = 100;
	private int pointValue = 200;

	private int activeComponents = 3;
	private bool dead = false;


	private float projectile_velocity = 5f;
	private float shotsPerSecond = 5f;
	public int projectile_damage = 2;

	void Damage(int amount) {
		hitPoints -= amount;
		if (hitPoints <= 0 && !dead) {
			//Debug.Log ("Destroy boss!");
			GameController.GetInstance ().ScorePoints (pointValue);
			dead = true;
			GameController.GetInstance ().QueueLevelEnd ();
		} else {
			//animate damage?
		}
	}

	bool CheckComponents(bool fire = false) {
		bool hasComponents = false;
		foreach (Transform component in transform) {
			if (component.GetComponent<BossComponent> ()) {
				hasComponents = true;
				if (fire) {
					component.GetComponent<BossComponent> ().HandleShooting ();
				}
			}
		}
		return hasComponents;
	}

	public void ComponentDestroyed() {
		activeComponents--;
		if (activeComponents <= 0) {
			//switch modes, brighten color, desperation attacks.
			transform.GetComponent<Animator>().SetTrigger("Phase 2");
		}
	}

	void Update () {
		//check to see if components are still alive
		if (!CheckComponents (true)) { //this function call will tell the components to fire off, if they exist
			// if they are not, use desperation attacks until dead
			HandleShooting();
		}
	}

	public void HandleShooting() {

		float probability = Time.deltaTime * shotsPerSecond;

		if (Random.value < probability) { 
			FireFullSpread ();
		}
	}

	void FireFullSpread() {
		int i = Mathf.FloorToInt(Random.Range (-1f, 2f));
		GameObject bullet = ProjectileContainer.GetInstance ().nextAvailableBigProjectile ();
		if (bullet) {
			float y_offset = i * 1f;
			bullet.transform.position = transform.position + new Vector3 (0, y_offset);
			bullet.transform.rotation = Quaternion.identity;
			bullet.SetActive (true);
			Vector3 bulletVelocity = new Vector3 (-projectile_velocity, y_offset, 0);
			bullet.GetComponent<Rigidbody2D> ().velocity = bulletVelocity;
			bullet.GetComponent<Projectile> ().setDamageValue (projectile_damage);
			bullet.transform.SetParent (ProjectileContainer.GetInstance ().transform);
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		//detect bullets or physical collisions
		if (col.gameObject.GetComponent<Projectile> ()) {
			if (col.gameObject.GetComponent<Projectile> ().getDamageActive ()) {
				if (!CheckComponents ()) { 
					//doesn't directly take damage until all components are destroyed... is this accurate? 
					// Might need to swap things around a bit, and have the loss of a component just chunk off a noticeable bit of health
					// so the player can eventually whittle down any boss by shooting it directly...
					GameController.GetInstance ().ContinueCombo ();
					Damage (col.gameObject.GetComponent<Projectile> ().getDamageValue ());
					col.gameObject.GetComponent<Projectile> ().Destroy ();
				}
			}
		}
	}
}
