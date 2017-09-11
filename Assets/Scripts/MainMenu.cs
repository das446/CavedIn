using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static readonly string EnemyType="EnemeyType";
	public InputField player1;
	public InputField player2;
	public GameObject HelpScreen;
	public GameObject mainMenu;

	void Start(){
		if(PlayerPrefs.HasKey("Player1Name")){
			player1.text=PlayerPrefs.GetString("Player1Name");
		}
		if (PlayerPrefs.HasKey("Player2Name"))
        {
            player2.text = PlayerPrefs.GetString("Player2Name");
        }

	}
	public void GoToHelp(){
		mainMenu.SetActive(false);
		HelpScreen.SetActive(true);
	}
	public void GoBackToMain(){
		HelpScreen.SetActive(false);
		mainMenu.SetActive(true);
	}
	public void GoToLobby(){
		SceneManager.LoadScene(2);
	}

	public void GoToVsLocal(){
		PlayerPrefs.SetString(EnemyType,"Human");
		GoToMainScene();
	}

	public void GoToVsCom(){
		PlayerPrefs.SetString(EnemyType, "Com");
        GoToMainScene();
	}

	void GoToMainScene(){
		PlayerPrefs.SetString("Player1Name",player1.text);
		PlayerPrefs.SetString("Player2Name",player2.text);
		SceneManager.LoadScene(1);
	}
}
