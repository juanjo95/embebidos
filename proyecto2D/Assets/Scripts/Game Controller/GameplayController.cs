using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	public bool gameInProgress;
	public Transform player;
	public CameraFollow camera;
	public Text scoreText, shotText, highscore;
	public GameObject gameOverPanel, gameWinPanel, pausePanel;

	[HideInInspector]
	public bool lastShot;

	private bool gameFinished, checkGameStatus;
	private List<GameObject> enemies;
	private List<GameObject> objects;
	private float timeAfterLastShot, distance, time, timeSinceStartedShot;
	private int prevLevel;

	void Awake(){
		CreateInstance ();
	}

	// Use this for initialization
	void Start () {
		InitializeVariables ();

		if (GameController.instance != null && MusicController.instance != null) {
			if (GameController.instance.isMusicOn) {
				MusicController.instance.GameplaySound ();
			} else {
				MusicController.instance.StopAllSounds ();
			}
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (gameInProgress) {
			GameIsOnPlay ();
			DistanceBetweenCannonAndBullet ();
		}


		if(GameController.instance != null){
			UpdateGameplayController ();
		}
			
	}

	void CreateInstance(){
		if(instance == null){
			instance = this;
		}
	}

	void UpdateGameplayController(){
		scoreText.text = GameController.instance.score.ToString("N0");
		shotText.text = "X" + PlayerBullet ();
	}

	void InitializeVariables(){
		gameInProgress = true;
		enemies = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Enemy"));
		objects = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Object"));
		distance = 10f;
		if(GameController.instance != null){
			GameController.instance.score = 0;
			prevLevel = GameController.instance.currentLevel;
			highscore.transform.GetChild (0).transform.GetComponent<Text> ().text = GameController.instance.highscore [GameController.instance.currentLevel - 1].ToString ("N0");

			if(GameController.instance.highscore[GameController.instance.currentLevel - 1] > 0){
				highscore.gameObject.SetActive (true);
			}

		}
			
	}
		
	void GameIsOnPlay(){
		/*if (PlayerBullet () == 0) {
			timeAfterLastShot += Time.deltaTime;
			camera.isFollowing = false;
			if (timeAfterLastShot > 2f) {
				if (AllStopMoving () && AllEnemiesDestroyed ()) {
					if (!gameFinished) {
						gameFinished = true;
						Debug.Log ("Hello World");
					}
				} else if (AllStopMoving () && !AllEnemiesDestroyed ()) {
					if (!gameFinished) {
						gameFinished = true;
						Debug.Log ("Hi World");
					}
				}
			}

		}*/

		if(checkGameStatus){
			timeAfterLastShot += Time.deltaTime;
			if (timeAfterLastShot > 2f) {
				if (AllStopMoving () || Time.time - timeSinceStartedShot > 8f) {
					if (AllEnemiesDestroyed ()) {
						if (!gameFinished) {
							gameFinished = true;
							GameWin ();
							timeAfterLastShot = 0;
							checkGameStatus = false;
						}
					} else {
						if (PlayerBullet () == 0) {
							if (!gameFinished) {
								gameFinished = true;
								timeAfterLastShot = 0;
								checkGameStatus = false;
								GameLost ();
							}
						} else {
							checkGameStatus = false;
							camera.isFollowing = false;
							timeAfterLastShot = 0;
						}
					}
				}
			}

		}

	}

	void GameWin(){
		if(GameController.instance != null && MusicController.instance != null){
			if(GameController.instance.isMusicOn){
				AudioSource.PlayClipAtPoint (MusicController.instance.winSound, Camera.main.transform.position);
			}

			if(GameController.instance.score > GameController.instance.highscore[ GameController.instance.currentLevel - 1]){
				GameController.instance.highscore [ GameController.instance.currentLevel - 1] = GameController.instance.score;
			}

			highscore.text = GameController.instance.highscore [GameController.instance.currentLevel].ToString ("N0");

			int level = GameController.instance.currentLevel;
			level++;
			if(!(level-1 >= GameController.instance.levels.Length)){
				GameController.instance.levels [level - 1] = true;
			}

			GameController.instance.Save ();
			GameController.instance.currentLevel = level;
		}
		gameWinPanel.SetActive (true);

	}

	void GameLost(){
		if(GameController.instance != null && MusicController.instance != null){
			if(GameController.instance.isMusicOn){
				AudioSource.PlayClipAtPoint (MusicController.instance.loseSound, Camera.main.transform.position);
			}
		}
		gameOverPanel.SetActive (true);
	}


	public int PlayerBullet(){
		int playerBullet = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).transform.GetComponent<Cannon> ().shot;
		return playerBullet;
	}



	private bool AllEnemiesDestroyed(){
		return enemies.All(x => x == null);
	}


	private bool AllStopMoving(){
		foreach (var item in objects.Union(enemies)) {
			if(item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 0){
				return false;
			}
				
		}

		return true;
	}

	void DistanceBetweenCannonAndBullet(){
		GameObject[] bullet = GameObject.FindGameObjectsWithTag ("Player Bullet");
		foreach (GameObject distanceToBullet in bullet) {
			if (!distanceToBullet.transform.GetComponent<CannonBullet> ().isIdle) {
				if (distanceToBullet.transform.position.x - player.position.x > distance) {
					camera.isFollowing = true;
					checkGameStatus = true;
					timeSinceStartedShot = Time.time;
					TimeSinceShot ();
					camera.target = distanceToBullet.transform;
				} else {
					if(PlayerBullet() == 0){
						camera.isFollowing = true;
						checkGameStatus = true;
						timeSinceStartedShot = Time.time;
						TimeSinceShot ();
						camera.target = distanceToBullet.transform;
					}
				}
			}
		}
		/*if (GameObject.FindGameObjectWithTag ("Player Bullet") != null) {
			if (!GameObject.FindGameObjectWithTag ("Player Bullet").transform.GetComponent<CannonBullet> ().isIdle) {
				Transform distanceToBullet = GameObject.FindGameObjectWithTag ("Player Bullet").transform;
				if (distanceToBullet.position.x - player.position.x > distance) {
					camera.isFollowing = true;
					checkGameStatus = true;
					TimeSinceShot ();
					camera.target = distanceToBullet;
				}
			}
				
		}*/
	}

	void TimeSinceShot(){
		time += Time.deltaTime;
		if (time > 3f) {
			time = 0f;
			GameObject.FindGameObjectWithTag ("Player Bullet").transform.GetComponent<CannonBullet> ().isIdle = true;
		}
			
	}

	public void RestartGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		if(GameController.instance != null){
			GameController.instance.currentLevel = prevLevel;
		}
	}

	public void BackToLevelMenu(){
		if(GameController.instance != null && MusicController.instance != null){
			if (GameController.instance.isMusicOn) {
				MusicController.instance.PlayBgMusic ();
			} else {
				MusicController.instance.StopAllSounds ();
			}
		}
		SceneManager.LoadScene ("Level Menu");
		Time.timeScale = 1;
	}

	public void ContinueGame(){
		if (GameController.instance != null) {
			SceneManager.LoadScene ("Level " + GameController.instance.currentLevel);
		}
	}

	public void PauseGame(){
		if (gameInProgress) {
			gameInProgress = false;
			Time.timeScale = 0;
			pausePanel.SetActive (true);
		}
	}

	public void ResumeGame(){
		Time.timeScale = 1;
		gameInProgress = true;
		pausePanel.SetActive (false);
	}


		

}
