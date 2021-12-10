using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private float current_z_angle = 0;
	private float target_z_angle = 0;
	private float turn_speed = 5f;

	private static CameraController instance = null;

	public static CameraController GetInstance() {
		return instance;
	}

	public void RotateCamera (string direction) {
		switch (direction) {
		case "up":
			target_z_angle = -10;
			break;
		case "mid":
			target_z_angle = 0;
			break;
		case "down":
			target_z_angle = 10;
			break;
		}
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	void Update () {
		//current_z_angle = transform.rotation.z;
		//Debug.Log ("current = " + current_z_angle + ", target = " + target_z_angle);
		if (current_z_angle != target_z_angle) {
			if (current_z_angle > target_z_angle) {
				current_z_angle -= turn_speed * Time.deltaTime;
			} else if (current_z_angle < target_z_angle) {
				current_z_angle += turn_speed * Time.deltaTime;
			}

			if (Mathf.Abs (current_z_angle - target_z_angle) < 1) {
				current_z_angle = target_z_angle;
			}

			Quaternion target = Quaternion.Euler (0, 0, current_z_angle);
			transform.rotation = Quaternion.Lerp (transform.rotation, target, Time.deltaTime * 2f); // this is where one problem is. In some cases, this will stop calling even tho the interpolation is only at the beginning instead of the end....
		}
	}
}
