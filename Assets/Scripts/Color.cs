using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caved{
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

	public static Color Random(){
		Color[] colors=new Color[]{Red,Blue,Green,Black,White,Yellow};
		int r = UnityEngine.Random.Range(0, colors.Length);
		return colors[r];
	}

	public static implicit operator UnityEngine.Color(Color c){
		return c.rgb;
	}

	public static Color FromName(string Name){

		//Color[] colors = new Color[] { Red, Blue, Green, Black, White, Yellow,Orange };
		Dictionary<string, Color> dictionary=new Dictionary<string,Color>{
			{"Red",Red},
			{"Blue",Blue},
			{"Green",Green},
			{"White",White},
			{"Black",Black},
			{"Yellow",Yellow},
			{"Orange",Orange}
		};
		return dictionary[Name];


	}
}
}