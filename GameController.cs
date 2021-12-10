using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	/* GameController should handle all global things, level load triggers, etc.
	 * Score, combo level
	*/

	public Text scoreboard;

	private int player_score = 0;
	private float combo_level = 1f;
	private float combo_increment = 0.1f;
	private float combo_decay = 0.1f;
	private float combo_decay_delay = 1f;
	private float combo_decay_tick = 0.5f;

	private static GameController instance = null;

	private bool endQueued = false;

	//private int current_level = 0;

	public static GameController GetInstance() {
		return instance;
	}

	public void LoadLevel (string level) {
		SceneManager.LoadScene (level);
	}

	public void LoadLevelByNumber(int levelNum) {
		//current_level = levelNum;
		SceneManager.LoadScene ("Level_" + levelNum.ToString ("D2"));
	}

	public void GameOver() {
		SceneManager.LoadScene ("Game Over");
	}

	public void GameOverReset() {
		player_score = 0;
		combo_level = 1;
		SceneManager.LoadScene ("Main Menu");
	}

	public int ScorePoints (int amount) {
		int actual_amount = amount * Mathf.FloorToInt (combo_level);
		player_score += actual_amount;
		combo_level += combo_increment;
		combo_level = float.Parse(combo_level.ToString ("f1"));
		CancelInvoke ("HandleComboDecay"); //kill the old one before starting a new one to make sure the delay kicks in.
		InvokeRepeating ("HandleComboDecay", combo_decay_delay, combo_decay_tick);
		UpdateScoreboard ();
		return player_score;
	}

	public void ContinueCombo () {
		//this should get called whenever an enemy is hit, but not destryoed. Doesn't increase the combo, but re-sets the decay timer.
		CancelInvoke ("HandleComboDecay");
		InvokeRepeating ("HandleComboDecay", combo_decay_delay, combo_decay_tick);
	}

	public void EndCombo (bool clean = false) {
		if (!clean) {
			//damage taken, don't cash out combo, just re-set value
			combo_level = 1;
			UpdateScoreboard ();
			CancelInvoke ("HandleComboDecay");
		} else {
			//if we ever have some method of 'cashing out' a combo, to score bigger points, this is where it would go.
		}
	}

	public int GetCurrentLevel() {
		int level = int.Parse(SceneManager.GetActiveScene ().name.Split ('0') [1]);
		return level;
	}

	void HandleComboDecay () {
		//every tenth of a second, if the player hasn't scored recently, have the combo level tick down.
		if (combo_level > 1f) {
			combo_level -= combo_decay;
			combo_level = float.Parse(combo_level.ToString ("f1"));
		} else {
			//else we're at or below 1.
			combo_level = 1;
			CancelInvoke ("HandleComboDecay");
		}
		UpdateScoreboard ();
	}

	private void UpdateScoreboard () {
		if (scoreboard != null) {
			scoreboard.text = "Score: " + player_score + "\n"
							+ "Combo: " + combo_level;
		}
	}

	private void EndPlayLevel () {
		//if the level is over (whether from the boss dying or whatever), load the next cutscene / level
		if (endQueued) {
			//SceneManager.LoadScene ("Cutscene_" + current_level.ToString ("D2"));
			SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex + 1);
		}
	}

	public void QueueLevelEnd() {
		//rather than forcing a hard end, this will cause some kind of 'fade out' and then switch scenes after a delay\
		if (!endQueued) {
			//Debug.Log ("Queue level end in 2 seconds");
			endQueued = true;
			Invoke ("EndPlayLevel", 2f);
		} else {
			//Debug.Log ("End already queued, ignore call");
		}
	}

	public void EndCutsceneLevel () {
		//call this from... somewhere when the cutscene ends (or the player presses the 'skip' button)
		if (SceneManager.GetActiveScene ().buildIndex == 7) {
			//end of cutscene 3, go to win screen
			SceneManager.LoadScene (9);
		} else {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		}
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		if (TerrainController.GetInstance ()) {
			TerrainController.GetInstance ().StartScrolling ();
		}
	}

	void OnLevelWasLoaded(int level) {
		//Debug.Log ("Loaded level " + level);
		if ( level == 9) {
			CutsceneController.GetInstance ().DisplayFinalScore (player_score);
		} else if (level == 2 || level == 4 || level == 6) {
			//grab non-persistent elements from the new scene into usable references.
			LoadScoreboard ();
			LoadEnemyController ();
			endQueued = false;
		}
	}

	void LoadEnemyController() {
		if (EnemyController.GetInstance ()) {
			//Debug.Log ("Loaded enemyController. Level index = "+SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex););
			EnemyController.GetInstance ().LoadNewLevel (int.Parse(SceneManager.GetActiveScene ().name.Split ('0') [1]));
		} else {
			//Debug.Log ("Failed to load enemyController. try again.");
			Invoke ("LoadEnemyController", 0.5f); //if it fails to load, give it half a second before trying again.
		}
	}

	bool LoadScoreboard() {
		if (GameObject.Find ("Scoreboard")) {
			scoreboard = GameObject.Find ("Scoreboard").GetComponent<Text>();
			UpdateScoreboard ();
			return true;
		}
		return false;
	}

}
