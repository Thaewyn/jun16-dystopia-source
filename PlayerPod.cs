using UnityEngine;
using System.Collections;

public class PlayerPod : MonoBehaviour {

	private float projectile_velocity = 14f;
	private int projectile_damage = 1;

	private Vector3 projectile_offset;

	void Start () {
		if (gameObject.name == "Left Pod") {
			projectile_offset = new Vector3 (0.1f, 0.4f);
		} else {
			projectile_offset = new Vector3 (0.1f, -0.4f);
		}
	}

	public void Activate () {
		gameObject.SetActive (true);
	}

	public void Deactivate () {
		gameObject.SetActive (false);
	}

	public void Fire() {
		GameObject bullet = ProjectileContainer.GetInstance().nextAvailableProjectile(true);
		bullet.transform.position = transform.position + projectile_offset;
		bullet.transform.rotation = Quaternion.identity;
		bullet.SetActive (true);
		bullet.GetComponent<Rigidbody2D> ().velocity = new Vector3 (projectile_velocity, 0, 0);
		bullet.GetComponent<Projectile> ().setDamageValue(projectile_damage);
	}

	public void SetProjectileDamage (int newValue) {
		projectile_damage = newValue;
	}

	public void setProjectileVelocity (float newSpeed) {
		projectile_velocity = newSpeed;
	}
}
