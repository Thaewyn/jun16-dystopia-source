using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public GameObject[] carPrefabs;
	public GameObject[] bossPrefabs;

	private static EnemyController instance = null;

	private bool bossMode = false;

	public static EnemyController GetInstance() {
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
		BuildLevels();
	}

	public bool SpawnEnemy(int whichCar, int whichPath) {
		//Debug.Log ("Spawn car type " + which);

		Vector3 spawn_location = new Vector3 (-1, -1, 0);
		GameObject enemy = Instantiate (carPrefabs[whichCar-1], spawn_location, Quaternion.identity) as GameObject;
		if (enemy) {
			enemy.transform.SetParent (transform);
			enemy.GetComponent<Enemy> ().SetPath (whichPath);
			return true;
		} else {
			//instantiate didn't work?
			return false;
		}
	}

	private bool SpawnBoss(int whichBoss) {
		Vector3 spawn_location = new Vector3 (6.5f, 3f, 0f);
		GameObject enemy = Instantiate (bossPrefabs[0], spawn_location, Quaternion.identity) as GameObject;
		if (enemy) {
			//handle any other setup-y things

			return true;
		} else {
			return false;
		}
	}

	public void HandleSpawns (int level, int tileCounter) {
		List<int[]> current_level = level_01;
		//int[] current_step = level_01 [level_spawn_step];
		if (level == 2) {
			current_level = level_02;
			//current_step = level_02 [level_spawn_step];
		} else if (level == 3) {
			current_level = level_03;
			//current_step = level_03 [level_spawn_step];
		}
		for (int i = 0; i < current_level.Count; i++) {
			if (tileCounter >= current_level [i] [0] && tileCounter <= current_level [i] [1]) {
				switch (current_level [i] [1]) {
				case 999:
					//boss
					if (!bossMode) {
						//Debug.Log ("HandleSpawns - SpawnBoss");
						SpawnBoss (level);
						bossMode = true;
					}
					break;
				case 1111:
					//time's up... force level end
					GameController.GetInstance ().QueueLevelEnd();
					break;
				case 2222:
					//camera control
					if (tileCounter == current_level [i] [0]) {
						string direction = "mid";
						if (current_level [i] [2] == 1) {
							direction = "up";
						} else if (current_level [i] [2] == -1) {
							direction = "down";
						}
						CameraController.GetInstance ().RotateCamera (direction);
					}
					break;
				case 3333:
					//in-level text: [2] = which string (see CutsceneController), [3] = color preset
					if (tileCounter == current_level [i] [0]) {
						int whichLevel = GameController.GetInstance ().GetCurrentLevel ();
						CutsceneController.GetInstance ().PlayLevelSpeech (whichLevel, current_level [i] [2], current_level [i] [3]);
					}
					break;
				default:
					//spawn enemy
					SpawnEnemy (current_level [i] [2], current_level [i] [3]);
					break;
				}
			}
		}
	}

	public bool LoadNewLevel(int whichLevel){
		//Debug.Log ("EnemyController.LoadNewLevel whichLevel = " + whichLevel);
		//BuildLevel (whichLevel);
		bossMode = false;
		//level_spawn_step = 0;
		return true;
	}

	//private int[][] enemy_list;
	//private int level_spawn_step = 0;

	private List<int[]> level_01 = new List<int[]>();
	private List<int[]> level_02 = new List<int[]>();
	private List<int[]> level_03 = new List<int[]>();

	private bool BuildLevels() {
		//data format: Startpanel, endpanel (or special), enemytype, formation/path

		level_01.Add( new int[] { 78, 78, 1, 16 });
		level_01.Add( new int[] { 79, 79, 1, 17 });
		level_01.Add( new int[] { 87, 87, 1, 1 });
		level_01.Add( new int[] { 88, 88, 1, 3 });
		level_01.Add( new int[] { 89, 92, 2, 3 });
		level_01.Add( new int[] { 90, 90, 1, 4 });
		level_01.Add( new int[] { 91, 91, 3, 12 });
		level_01.Add( new int[] { 92, 92, 3, 13 });
		level_01.Add( new int[] { 95, 98, 1, 5 });
		level_01.Add( new int[] { 100, 103, 2, 6 });
		level_01.Add( new int[] { 106, 109, 1, 7 });
		//boss
		level_01.Add( new int[] { 120, 999, 0, 0 });
		//camera control
		level_01.Add( new int[] { 86, 2222, 1, 0 });
		level_01.Add( new int[] { 100, 2222, -1, 0 });
		level_01.Add( new int[] { 125, 2222, 0, 0 });
		//force level end
		level_01.Add( new int[] { 300, 1111, 0, 0 });
		//speech
		level_01.Add( new int[] { 4, 3333, 1, 3 });
		level_01.Add( new int[] { 12, 3333, 2, 3 });
		level_01.Add( new int[] { 20, 3333, 3, 2 });
		level_01.Add( new int[] { 28, 3333, 4, 3 });
		level_01.Add( new int[] { 36, 3333, 5, 2 });
		level_01.Add( new int[] { 44, 3333, 6, 3 });
		level_01.Add( new int[] { 52, 3333, 7, 1 });
		level_01.Add( new int[] { 60, 3333, 8, 1 });
		level_01.Add( new int[] { 68, 3333, 9, 1 });
		level_01.Add( new int[] { 76, 3333, 10, 3 });
		level_01.Add( new int[] { 84, 3333, 11, 2 });
		level_01.Add( new int[] { 120, 3333, 12, 3 });

		//Level 2. 
		level_02.Add( new int[] { 10, 10, 1, 16 });
		level_02.Add( new int[] { 11, 11, 1, 17 });
		level_02.Add( new int[] { 17, 17, 1, 7 });
		level_02.Add( new int[] { 18, 18, 1, 6 });
		level_02.Add( new int[] { 19, 22, 2, 5 });
		level_02.Add( new int[] { 20, 20, 1, 4 });
		level_02.Add( new int[] { 25, 28, 1, 3 });
		level_02.Add( new int[] { 30, 33, 2, 2 });
		level_02.Add( new int[] { 36, 39, 1, 1 });
		level_02.Add( new int[] { 40, 42, 1, 0 });
		//boss
		level_02.Add( new int[] { 50, 999, 0, 0 });
		//camera control
		level_02.Add( new int[] { 16, 2222, -1, 0 });
		level_02.Add( new int[] { 30, 2222, 0, 0 });
		//force level end
		level_02.Add( new int[] { 200, 1111, 0, 0 });
		//speech bubble
		/*level_02.Add( new int[] { 4, 3333, 1, 1 });
		level_02.Add( new int[] { 12, 3333, 2, 3 });
		level_02.Add( new int[] { 20, 3333, 3, 2 });
		level_02.Add( new int[] { 28, 3333, 4, 1 });
		level_02.Add( new int[] { 36, 3333, 5, 2 });
		level_02.Add( new int[] { 44, 3333, 6, 3 });
		level_02.Add( new int[] { 52, 3333, 7, 2 });*/
		level_02.Add( new int[] { 50, 3333, 1, 3 });

		//Level 3. Speech bubbles end around 60
		level_03.Add( new int[] { 70, 70, 1, 16 });
		level_03.Add( new int[] { 71, 71, 1, 17 });
		level_03.Add( new int[] { 77, 77, 1, 1 });
		level_03.Add( new int[] { 78, 78, 1, 3 });
		level_03.Add( new int[] { 79, 82, 1, 3 });
		level_03.Add( new int[] { 80, 80, 1, 12 });
		level_03.Add( new int[] { 85, 88, 1, 7 });
		level_03.Add( new int[] { 90, 93, 1, 6 });
		level_03.Add( new int[] { 96, 99, 1, 8 });
		level_03.Add( new int[] { 80, 80, 1, 13 });
		level_03.Add( new int[] { 100, 100, 1, 15 });
		level_03.Add( new int[] { 100, 100, 1, 14 });
		level_03.Add( new int[] { 100, 100, 1, 10 });
		level_03.Add( new int[] { 105, 108, 1, 0 });
		level_03.Add( new int[] { 105, 108, 1, 1 });
		//boss
		level_03.Add( new int[] { 110, 999, 0, 0 });
		//camera controls
		level_03.Add( new int[] { 16, 2222, 1, 0 });
		level_03.Add( new int[] { 30, 2222, 0, 0 });
		//force level end
		level_03.Add( new int[] { 250, 1111, 0, 0 });
		//speech bubble stuff
		level_03.Add( new int[] { 4, 3333, 1, 3 });
		level_03.Add( new int[] { 12, 3333, 2, 2 });
		level_03.Add( new int[] { 20, 3333, 3, 3 });
		level_03.Add( new int[] { 28, 3333, 4, 2 });
		level_03.Add( new int[] { 36, 3333, 5, 3 });
		level_03.Add( new int[] { 44, 3333, 6, 2 });
		level_03.Add( new int[] { 52, 3333, 7, 2 });
		level_03.Add( new int[] { 60, 3333, 8, 3 });
		level_03.Add( new int[] { 110, 3333, 9, 3 });

		return true;
	}
}
