using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//public GameObject projectilePrefab;

	private GameController gameController;
	private static Player instance = null;

	//movement and speed
	private float x_speed = 4f;
	private float y_speed = 4f;
	private float x_min;
	private float x_max;
	private float y_min;
	private float y_max;
	private float x_padding = 0.3f; // edge padding, in world units. This is relative to the player's centerpoint.
	private float y_padding = 1.2f;
	//weapons and projectiles
	private int projectile_index = 0;
	private float fire_rate = 0.05f;
	private float projectile_velocity = 14f;
	private int projectile_damage = 1;
	//health, damage, and i-frames
	private int hp = 6;
	private bool invulnerable = false;
	private float invuln_timer = 1f;

	private PlayerPod[] pods;
	private bool podsActive = false;

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	public static Player GetInstance() {
		return instance;
	}

	void Start () {
		gameController = GameController.GetInstance ();
		//float distance = transform.position.z - Camera.main.transform.position.z;
		//Vector3 botLeft = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		//Vector3 topRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distance));
		x_min = 0f + x_padding;
		x_max = 8f - x_padding;
		y_min = 0f + y_padding;
		y_max = 6f - y_padding;

		pods = transform.GetComponentsInChildren<PlayerPod> ();
		if (gameController.GetCurrentLevel () != 2) {
			foreach (PlayerPod pod in pods) {
				pod.Deactivate ();
			}
		} else {
			ActivatePods ();
		}
	}
	
	void Update () {
		HandleMovement ();

		HandleWeapons ();
	}

	void HandleMovement() {
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position += Vector3.right * x_speed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += Vector3.left * x_speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.position += Vector3.up * y_speed * Time.deltaTime;
			//transform.GetComponent<Animator> ().SetTrigger ("Move Up"); //not particularly viable in this specific case, but good to know that they work this way...
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position += Vector3.down * y_speed * Time.deltaTime;
			//transform.GetComponent<Animator> ().SetTrigger ("Move Down");
		}

		//screenfix
		float fixed_x = Mathf.Clamp(transform.position.x, x_min, x_max);
		float fixed_y = Mathf.Clamp(transform.position.y, y_min, y_max);
		transform.position = new Vector2 (fixed_x, fixed_y);
	}

	void HandleWeapons() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("FireBasic", 0.0001f, fire_rate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("FireBasic");
		}
	}

	void FireBasic() {
		//fire basic projectile
		projectile_index++;
		//Debug.Log("Fire basic projectile id="+projectile_index);
		//GameObject bullet = Instantiate (projectilePrefab, transform.position, Quaternion.identity) as GameObject;
		GameObject bullet = ProjectileContainer.GetInstance().nextAvailableProjectile(true);
		float side = 1f;
		if (projectile_index % 2 == 1) {
			side = -1f;
		}
		Vector3 offset = new Vector3 (0.5f, 0.15f * side);
		bullet.transform.position = transform.position + offset;
		bullet.transform.rotation = Quaternion.identity;
		bullet.SetActive (true);
		bullet.GetComponent<Rigidbody2D> ().velocity = new Vector3 (projectile_velocity, 0, 0);
		bullet.GetComponent<Projectile> ().setDamageValue(projectile_damage);
		//bullet.transform.SetParent (ProjectileContainer.GetInstance ().transform);
		if (podsActive) {
			if (projectile_index % 2 == 0) {
				pods [0].Fire ();
			} else {
				pods [1].Fire ();
			}
		}

	}

	void ActivatePods () {
		foreach (PlayerPod pod in pods) {
			pod.Activate ();
			podsActive = true;
		}
	}

	void Damage (int amount) {
		//Debug.Log ("Damage received: " + amount);
		hp -= amount;
		if (hp <= 0) {
			//Debug.Log ("Player Dead. Game Over");
			gameController.GameOver ();
		} else {
			//animate receiving damage?
			invulnerable = true;
			transform.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
			gameController.EndCombo (false);
			if (hp <= 2) {
				HudController.GetInstance ().DangerTrigger ();
			}
			HudController.GetInstance ().DamageTrigger ();
			Invoke ("EndInvuln", invuln_timer);
		}
	}

	void EndInvuln() {
		invulnerable = false;
		transform.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
	}

	void OnTriggerEnter2D (Collider2D col) {
		//hit by a bullet
		if (col.gameObject.GetComponent<Projectile> () && invulnerable == false) {
			Damage (col.gameObject.GetComponent<Projectile> ().getDamageValue());
		}
		//physical collision with an enemy
		if (col.gameObject.GetComponent<Enemy> () && invulnerable == false) {
			Damage (1);
		}
		if (col.gameObject.GetComponent<Bit> () && invulnerable == false) {
			Damage (1);
		}
	}
}
