using UnityEngine;
using System.Collections;

public class TrainCarEngine : TrainCar {

	public override void HandleDestruction() {
		//Debug.Log ("Destroy train engine");
		trainBoss.GetComponent<TrainBoss>().CarDestroyed (0);
	}

	public override void HandleAttackPattern(int counter) {
		//Debug.Log ("Attack Pattern for Engine: "+counter);
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
