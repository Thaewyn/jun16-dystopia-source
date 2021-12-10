using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

	private int hitPoints = 7;
	private int pointValue = 1;

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
		if (hitPoints <= 0) {
			GameController.GetInstance().ScorePoints(pointValue);
			Destroy (gameObject);
		}
	}
}
