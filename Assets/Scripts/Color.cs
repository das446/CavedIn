using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged{
	[System.Serializable]
public class Color {

	public static readonly Color Red=new Color("Red","Ruby",UnityEngine.Color.red);
	public static readonly Color Blue = new Color("Blue", "Saphire", UnityEngine.Color.blue);
	public static readonly Color Green = new Color("Green", "Emerald", UnityEngine.Color.green);
	public static readonly Color Yellow = new Color("Yellow", "Amber", UnityEngine.Color.yellow);
	public static readonly Color Black = new Color("Black", "Onyx", UnityEngine.Color.black);
	public static readonly Color White = new Color("White", "Diamond", UnityEngine.Color.white);
	public static readonly Color Orange = new Color("Orange", "Orangite", Colors.OrangeCrayola);

	public string name;
	public string GemName;

	public UnityEngine.Color rgb;

	public Color(string Name,string gem,UnityEngine.Color RGB){
		name=Name;
		GemName=gem;
		rgb=RGB;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Color Random(){
		Color[] colors=new Color[]{Red,Blue,Green,Black,White,Yellow};
		int r = UnityEngine.Random.Range(0, colors.Length);
		return colors[r];
	}

	public static implicit operator UnityEngine.Color(Color c){
		return c.rgb;
	}
}
}