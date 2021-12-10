using UnityEngine;
using System.Collections;

public class GarbageCollector : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		//projectile garbage cleanup
		if (col.gameObject.GetComponent<Projectile> ()) {
			//Destroy (col.gameObject);
			col.gameObject.GetComponent<Projectile> ().Destroy();
		}
	}
}
