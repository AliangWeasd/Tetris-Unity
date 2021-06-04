using UnityEngine;
using System.Collections;

public class Over : MonoBehaviour {
	public float time = 0f;
	public bool flashing = false;
	public bool hover = false;
	public bool gameLocked = false;
	public bool musicLocked = false;

	public Sprite[] states;

	void Update () {
		if (hover) {
			time += Time.deltaTime;
			if (time > 0.2f) {
				if (flashing) {
					GetComponent<SpriteRenderer> ().sprite = states [1];
					flashing = false;
				} else {
					GetComponent<SpriteRenderer> ().sprite = states [0];
					flashing = true;
				}
				time = 0f;
			}
		} else {
			GetComponent<SpriteRenderer> ().sprite = states [0];
			flashing = true;
			time = 1f;
		}
		if(!hover && (gameLocked || musicLocked))
			GetComponent<SpriteRenderer> ().sprite = states [1];
	}
}
