using UnityEngine;
using System.Collections;

public class getLevel : MonoBehaviour {
	public Sprite zero;

	void Start () {
		if (GameObject.Find ("Menu") != null)
			GameObject.Find ("6 Spriteblock").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("Menu").GetComponent<MenuMoveA> ().save;
		else
			GameObject.Find ("6 Spriteblock").GetComponent<SpriteRenderer> ().sprite = zero;
	}
}
