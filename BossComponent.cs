using UnityEngine;
using System.Collections;

public class BossComponent : MonoBehaviour {

	public Sprite[] damage_sprites;

	//public GameObject projectilePrefab;
	private float projectile_velocity = 7f;
	private float shotsPerSecond = 0.5f;

	private int hitPoints = 30;
	private int projectile_damage = 1;
	private int pointValue = 50;

	public void HandleShooting() {

		float probability = Time.deltaTime * shotsPerSecond;

		if (Random.value < probability) { //sometimes a very long frame can mean that probability is greater than one, but that just means this value will definitely be true
			Fire ();
		}
	}

	private void Fire () {
		//fire a projectile from here. Should only be called by the parent 'boss' object.

		//GameObject bullet = Instantiate (projectilePrefab, transform.position, Quaternion.identity) as GameObject;
		GameObject bullet = ProjectileContainer.GetInstance().nextAvailableProjectile();
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;
		bullet.SetActive (true);
		Vector3 bulletVelocity = new Vector3 (-projectile_velocity, 0, 0);
		bullet.GetComponent<Rigidbody2D> ().velocity = bulletVelocity;
		bullet.GetComponent<Projectile> ().setDamageValue(projectile_damage);
		bullet.transform.SetParent (ProjectileContainer.GetInstance ().transform);
	}

	void Damage(int amount) {
		hitPoints -= amount;
		if (hitPoints <= 0) {
			//Debug.Log ("Destroy boss component");
			GameController.GetInstance ().ScorePoints (pointValue);
			transform.GetComponentInParent<MutantBoss> ().ComponentDestroyed ();
			Destroy (gameObject);
		} else {
			//animate damage?
			transform.GetComponent<Animator> ().SetTrigger ("Damaged");
			// this works, but there are two problems. The trigger actually queues up mutiple instances of the animation, rather than re-startingn from scratch
			// second, it seems to slowly tween back from the end position of the 'damaged' state back to 'idle', so it's going to need another keyframe back at its origin...
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		//detect bullets or physical collisions
		if (col.gameObject.GetComponent<Projectile> ()) {
			if (col.gameObject.GetComponent<Projectile> ().getDamageActive ()) {
				Damage (col.gameObject.GetComponent<Projectile> ().getDamageValue ());
				col.gameObject.GetComponent<Projectile> ().Destroy ();
			}
		}
	}
}
