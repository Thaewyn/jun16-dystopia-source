using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private int damage = 1;
	private bool piercing = false;
	private bool damageActive = true;

	public int getDamageValue () {
		return damage;
	}

	public void setDamageValue (int value) {
		damage = value;
	}

	public bool getPiercing () {
		return piercing;
	}

	public void setPiercing (bool hasPierce) {
		piercing = hasPierce;
	}

	public void setColor (float red, float green, float blue, float alpha) {
		transform.GetComponent<SpriteRenderer> ().color = new Color (red, green, blue, alpha);
	}

	void OnEnable() {
		//any cleanup for re-set
		damageActive = true;
	}

	public bool getDamageActive() {
		return damageActive;
	}

	public void Destroy() {
		if (transform.GetComponent<Animator> ()) {
			damageActive = false;
			transform.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0f, 0f, 0f);
			transform.GetComponent<Animator> ().SetTrigger ("Hit");
			Invoke ("DestroyAfterDelay", 0.2f);
		} else {
			gameObject.SetActive (false);
		}
	}

	private void DestroyAfterDelay() {
		gameObject.SetActive (false);
	}

	void OnDisable() {
		//in case anything needs to happen when disabled.
	}
}
