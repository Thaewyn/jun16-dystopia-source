using UnityEngine;
using System.Collections;

public class TrainCar2 : TrainCar {

	public override void HandleDestruction() {
		//Debug.Log ("Destroy train car 2");
		trainBoss.GetComponent<TrainBoss>().CarDestroyed (2);
		Destroy (gameObject);
	}

	public override void HandleAttackPattern(int counter) {
		//Debug.Log ("Attack Pattern for Car 2: "+counter);
		int split = 0;
		foreach (Transform turret in transform) {
			//named such assuming that turrets are the only children...
			split++;
			if(turret.GetComponent<Turret>()){
				turret.GetComponent<Turret>().FacePlayer();
				turret.GetComponent<Turret>().HandleFiring(counter+split);
			}
		}
	}
}
