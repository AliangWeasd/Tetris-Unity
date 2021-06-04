using UnityEngine;
using System.Collections;

public class Occupied : MonoBehaviour {

	public Sprite type;

	string blockName = "";

	//Defines the original empty spriteblock.
	public Sprite original;

	public void DropMove(int a, int b){
		blockName = a + "," + b + "(Clone)";

		GameObject.Find (blockName).GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer>().sprite;
		GameObject.Find(blockName).tag = "Filled";
	}
}
