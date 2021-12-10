using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour {

	private float scroll_speed = 20f;
	private bool loop = true;
	private bool scroll = false;

	private static TerrainController instance = null;
	//private GameController gameController = null;

	//spawn scripting stuff
	private int segmentCounter = 0;

	public void StartScrolling() {
		if (scroll == false) {
			scroll = true;
		}
	}

	public void StopScrolling() {
		if (scroll == true) {
			scroll = false;
		}
	}

	public static TerrainController GetInstance() {
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

	void Start () {
		StartScrolling ();
		//gameController = GameController.GetInstance();
	}
	
	void Update () {
		if (scroll) {
			HandleLevelScroll ();
		}
	}

	void HandleLevelScroll() {
		foreach (Transform terrain in transform) {
			if (terrain.GetComponent<Road> ()) {
				terrain.transform.position += Vector3.left * scroll_speed * Time.deltaTime;
				if (terrain.transform.position.x <= -8 && loop) {
					//this is for street pieces that need to be re-used and looped (not sure if this is necessary yet).
					terrain.transform.position += Vector3.right * 48; //move this panel to the right 16 world units (2 screen widths, in theory). Probably need a more bulletproof method here.
				}
			} else if (terrain.GetComponent<Background> ()) {
				terrain.transform.position += Vector3.left * (scroll_speed/4) * Time.deltaTime;
				if (terrain.transform.position.x <= -8 && loop) {
					terrain.transform.position += Vector3.right * 48;
				}
				
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		//Debug.Log ("TerrainController triggerenter");
		if (col.gameObject.GetComponent<Road> ()) {
			segmentCounter++;
			EnemyController.GetInstance ().HandleSpawns (GameController.GetInstance().GetCurrentLevel(), segmentCounter);
		}

		//projectile garbage cleanup
		if (col.gameObject.GetComponent<Projectile> ()) {
			//Destroy (col.gameObject);
			col.gameObject.GetComponent<Projectile> ().Destroy();
		}
	}
}
