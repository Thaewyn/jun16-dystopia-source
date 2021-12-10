using UnityEngine;
using System.Collections;

public abstract class TrainCar : MonoBehaviour {

	//ok, first attempt at a parent class. This should be the main class for each one of the various TrainBoss cars.
	public GameObject trainBoss;

	private int hitPoints = 100;
	private int pointValue = 125;
	private bool dead = false;
		
	void OnTriggerEnter2D (Collider2D col) {
		//hit by projectile
		if (col.gameObject.GetComponent<Projectile> ()) {
			if (col.gameObject.GetComponent<Projectile> ().getDamageActive ()) {
				GameController.GetInstance ().ContinueCombo ();
				Damage (col.gameObject.GetComponent<Projectile> ().getDamageValue ());
				col.gameObject.GetComponent<Projectile> ().Destroy ();
			}
		}
	}

	void Damage (int amount) {
		hitPoints -= amount;
		if (hitPoints <= 0 && !dead) {
			GameController.GetInstance().ScorePoints(pointValue);
			dead = true;
			HandleDestruction ();
		}
	}

	public abstract void HandleDestruction (); //This method does not describe what happens with handleDestruction. Child classes must define that themselves (because it's different for each one)

	public abstract void HandleAttackPattern (int counter);
}
