using UnityEngine;
using System.Collections;

public class TrainCar3 : TrainCar {

	public override void HandleDestruction() {
		//Debug.Log ("Destroy train car 3");
		trainBoss.GetComponent<TrainBoss>().CarDestroyed (3);
		Destroy (gameObject);
	}

	public override void HandleAttackPattern(int counter) {
		//Debug.Log ("Attack Pattern for Car 3: "+counter);
		if (counter > 8) {
			transform.GetComponentInChildren<Turret> ().FacePlayer ();
			transform.GetComponentInChildren<Turret> ().HandleFiring (counter);
		}
	}
}
