using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static readonly string EnemyType="EnemeyType";
	public Text player1;
	public Text player2;
	public GameObject HelpScreen;

	public void GoToHelp(){
		HelpScreen.SetActive(true);
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
