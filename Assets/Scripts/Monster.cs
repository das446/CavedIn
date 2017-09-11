using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caved{
public class Monster : MonoBehaviour {

	// Use this for initialization
	int x,y;

	public AudioClip DestroySFX;
	public int Points=3;
	public static int totalMonsters=0;
	public Text Description;
	public Image Picture;

	void Start(){
		totalMonsters++;
	}
	public void GetCaptured(){
		Player.Current.points+=Points;
		totalMonsters--;
		GetComponent<AudioSource>().PlayOneShot(DestroySFX);
		Destroy(gameObject);
	}

	void OnMouseEnter()
	{
		//Description.text="Red Dragon\n3 pts\n When you defeat this take another turn";
	}

	void OnMouseExit()
	{
		//Debug.Log("MOuseExit");
		Picture.enabled=false;
		Description.text="";
		
	}

}
}