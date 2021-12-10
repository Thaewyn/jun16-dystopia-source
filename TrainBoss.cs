using UnityEngine;
using System.Collections;

public class TrainBoss : MonoBehaviour {

	//private int hitPoints = 20;
	private int pointValue = 100;
	private bool dead = false;

	public GameObject[] trainCars;

	private int segmentCounter = 0;
	private int carCount = 3;

	public void CarDestroyed(int whichCar) {
		//Debug.Log ("CarDestroyed called from car " + whichCar);
		if (whichCar == 0 && !dead) {
			GameController.GetInstance ().ScorePoints (pointValue);
			dead = true;
			GameController.GetInstance ().QueueLevelEnd ();
		} else {
			transform.GetComponent<Animator> ().SetTrigger ("Destroy car");
			carCount--;
			segmentCounter = 0;
		}
	}

	public void SegmentHit() {
		//this will be tracked by the Engine, and used as a trigger counter (same as TerrainController) to deal with the various attack patterns of each car.
		segmentCounter++;
		//Debug.Log ("SegmentCounter = " + segmentCounter);
		trainCars [carCount].GetComponent<TrainCar> ().HandleAttackPattern (segmentCounter);
	}

	void OnTriggerEnter2D (Collider2D col) {
		//hit by projectile
		if (col.gameObject.GetComponent<Road> ()) {
			SegmentHit ();
		}
	}
}
