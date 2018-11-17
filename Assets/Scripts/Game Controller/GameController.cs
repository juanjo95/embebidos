using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {
	public static GameController instance;

	public bool isMusicOn;
	public bool isGameStartedFirstTime;
	public bool[] levels;
	public int[] highscore;
	public int score;
	public int currentLevel;

	private GameData data;

	void Awake(){
		CreateInstance ();
	}

	// Use this for initialization
	void Start () {
		InitializeGameVariables ();
	}

	void CreateInstance(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void InitializeGameVariables(){
		Load ();


		if (data != null) {
			isGameStartedFirstTime = data.GetIsGameStartedFirstTime ();
		} else {
			isGameStartedFirstTime = true;
		}

		if (isGameStartedFirstTime) {
			isGameStartedFirstTime = false;
			isMusicOn = true;
			levels = new bool[15];
			highscore = new int[levels.Length];

			levels [0] = true;
			for (int i = 1; i < levels.Length; i++) {
				levels [i] = false;
			}

			for (int i = 0; i < highscore.Length; i++) {
				highscore [i] = 0;
			}

			data = new GameData ();

			data.SetIsMusicOn (isMusicOn);
			data.SetIsGameStartedFirstTime (isGameStartedFirstTime);
			data.SetHighScore (highscore);
			data.SetLevels (levels);

			Save ();

			Load ();
		} else {
			isGameStartedFirstTime = data.GetIsGameStartedFirstTime ();
			isMusicOn = data.GetIsMusicOn ();
			highscore = data.GetHighScore ();
			levels = data.GetLevels ();
		}

	}

	public void Save(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Create(Application.persistentDataPath + "/data.dat");

			if(data != null){
				data.SetIsMusicOn(isMusicOn);
				data.SetIsGameStartedFirstTime(isGameStartedFirstTime);
				data.SetHighScore(highscore);
				data.SetLevels(levels);
				bf.Serialize(file, data);
			}

		}catch(Exception e){
			Debug.LogException (e, this);
		}finally{
			if(file != null){
				file.Close ();
			}
		}
	}

	public void Load(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.persistentDataPath + "/data.dat", FileMode.Open);
			data = bf.Deserialize(file) as GameData;

		}catch(Exception e){
			Debug.LogException (e, this);
		}finally{
			if(file != null){
				file.Close ();
			}
		}
	}
}

[Serializable]
class GameData{
	private bool isGameStartedFirstTime;
	private bool isMusicOn;
	private bool[] levels;
	private int[]  highscore;

	public void SetIsGameStartedFirstTime(bool isGameStartedFirstTime){
		this.isGameStartedFirstTime = isGameStartedFirstTime;
	}

	public bool GetIsGameStartedFirstTime(){
		return this.isGameStartedFirstTime;
	}

	public void SetIsMusicOn(bool isMusicOn){
		this.isMusicOn = isMusicOn;
	}

	public bool GetIsMusicOn(){
		return this.isMusicOn;
	}

	public void SetHighScore(int[] highscore){
		this.highscore = highscore;
	}

	public int[] GetHighScore(){
		return this.highscore;
	}

	public void SetLevels(bool[] levels){
		this.levels = levels;
	}

	public bool[] GetLevels(){
		return this.levels;
	}
}
