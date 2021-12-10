using UnityEngine;
using System.Collections;

public class Boss3FormationController : MonoBehaviour {

	public GameObject bitPrefab;

	private float spawnDelayInSeconds = 0.2f;

	public void SpawnBits () {
		//spawn one at a time with a delay, using the 'next free position' function
		Transform next = NextFreePosition();
		if (next) {
			GameObject enemy = Instantiate (bitPrefab, next.position, Quaternion.identity) as GameObject;
			enemy.transform.SetParent (next);
			Invoke ("SpawnBits", spawnDelayInSeconds);
		} else {
			//no available position, signal 'push forward' animation
			transform.GetComponentInParent<Animator>().SetTrigger("Attack");
		}
	}

	Transform NextFreePosition() {
		foreach (Transform position in transform) {
			if (position.childCount == 0) {
				return position;
			}
		}
		return null;
	}
}
