using UnityEngine;
using System.Collections;

public class BitPosition : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, 0.25f);
	}
}
