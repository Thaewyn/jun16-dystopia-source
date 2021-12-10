using UnityEngine;
using System.Collections;

public class TrainCar1 : TrainCar {

	public override void HandleDestruction() {
		//Debug.Log ("Destroy train car 1");
		trainBoss.GetComponent<TrainBoss>().CarDestroyed (1);
		Destroy (gameObject);
	}

	public override void HandleAttackPattern(int counter) {
		//Debug.Log ("Attack Pattern for Car 1: "+counter);

		if (counter > 5 && counter % 3 == 0) {
			EnemyController.GetInstance ().SpawnEnemy (1, 8);
		}
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
