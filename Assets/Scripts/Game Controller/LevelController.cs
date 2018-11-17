using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
	public bool[] levels;
	public Button[] levelButtons;

	// Use this for initialization
	void Start () {
		InitilizeGameVariables ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("Escape")){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
		}
	}

	void InitilizeGameVariables(){
		if(GameController.instance != null){
			levels = GameController.instance.levels;

			for (int i = 1; i < levels.Length; i++) {
				if (levels [i]) {
					levelButtons [i].transform.GetChild (1).transform.gameObject.SetActive (false);
				} else {
					levelButtons [i].interactable = false;
				}
			}
		}

	}

	public void LevelSelect(){
		string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

		switch(level){
		case "Level 1":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 1;	
			}
		break;

		case "Level 2":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 2;	
			}
			break;

		case "Level 3":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 3;	
			}
			break;

		case "Level 4":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 4;	
			}
			break;

		case "Level 5":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 5;	
			}
			break;

		case "Level 6":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 6;	
			}
			break;

		case "Level 7":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 7;	
			}
			break;

		case "Level 8":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 8;	
			}
			break;

		case "Level 9":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 9;	
			}
			break;

		case "Level 10":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 10;	
			}
			break;

		case "Level 11":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 11;	
			}
			break;

		case "Level 12":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 12;	
			}
			break;

		case "Level 13":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 13;	
			}
			break;

		case "Level 14":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 14;	
			}
			break;

		case "Level 15":
			if(GameController.instance != null){
				GameController.instance.currentLevel = 15;	
			}
			break;

		}

		SceneManager.LoadScene (level);
			
	}
}
