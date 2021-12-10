using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private float xDiff;
	private float yDiff;

	public void FacePlayer() {
		xDiff = Player.GetInstance().transform.position.x - transform.position.x;
		yDiff = Player.GetInstance().transform.position.y - transform.position.y;
		float radians = Mathf.Atan2 (yDiff, xDiff);
		float degrees = radians * (180 / Mathf.PI) + 180;
		Quaternion target = Quaternion.Euler (0f, 0f, degrees);
		transform.rotation = target;
	}

	public void HandleFiring(int counter) {
		if (counter % 4 == 0) {
			Fire ();
		}
	}

	private void Fire() {
		GameObject bullet = ProjectileContainer.GetInstance().nextAvailableBigProjectile();
		bullet.transform.position = transform.position;
		bullet.transform.rotation = transform.rotation;
		bullet.SetActive (true);
		Vector3 bulletVelocity = new Vector3 (xDiff, yDiff, 0);
		bullet.GetComponent<Rigidbody2D> ().velocity = bulletVelocity;
		bullet.transform.SetParent (ProjectileContainer.GetInstance ().transform);
	}
}
