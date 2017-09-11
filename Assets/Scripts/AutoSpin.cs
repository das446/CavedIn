using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpin : MonoBehaviour {
	
	public float speed=1;
	public bool spin=true;
	public enum Axis{x,y,z};
	public Axis axis;
	// Update is called once per frame
	void Update () {
		if(spin){
			transform.Rotate(transform.forward *speed * Time.deltaTime);
		}
	}
}
