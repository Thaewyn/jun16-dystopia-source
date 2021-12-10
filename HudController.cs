using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

	private static HudController instance = null;

	public static HudController GetInstance() {
		return instance;
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	public void DamageTrigger() {
		//transform.GetComponent<Animator> ().SetTrigger ("Damage");
		transform.GetComponentInChildren<HudHpController>().GetComponent<Animator>().SetTrigger ("Damage");
	}
	public void DangerTrigger() {
		transform.GetComponent<Animator> ().SetTrigger ("Danger");
	}
	public void HealedTrigger() {
		//transform.GetComponent<Animator> ().SetTrigger ("Healed");
		transform.GetComponentInChildren<HudHpController>().GetComponent<Animator>().SetTrigger ("Healed");
	}
}
