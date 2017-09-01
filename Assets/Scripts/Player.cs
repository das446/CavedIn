using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caged{
public class Player : MonoBehaviour {

	public Tile[] Tiles;
	public Tile prefabTile;
	public int points;
	public string Name;
	public int HandSize;

	public Text PointsText;
	public float inHandY;

	public static Player Current;
	// Use this for initialization
	void Start () {
		Tiles=new Tile[HandSize];
		float mid=20f;
		for(int i=0;i<HandSize;i++){
			Tiles[i]=Instantiate(prefabTile,new Vector2(mid+5*i,inHandY),Quaternion.identity);
			Tiles[i].RandomizeColors();
			Tiles[i].Display.AdjustDisplay();
			Tiles[i].GetComponent<InHandTile>().enabled=true;
			Tiles[i].GetComponent<InHandTile>().Controller=this;
		}
		Tiles[0].GetComponent<InHandTile>().Select();
	}
	
	// Update is called once per frame
	void Update () {
		if(Current==null){
			Current=this;
		}
		PointsText.text=Name+":"+points;
	}
}
}