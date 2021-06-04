using UnityEngine;
using System.Collections;

public class Stay : MonoBehaviour {

	// Updates the numbers shown in the highscores after the gameplay is done.
	void Start () {
		if (this.name == "1") {
			GetComponent<Score> ().Redigit (GameObject.Find ("First").GetComponent<NameAndScore> ().score);
		} else if (this.name == "2") {
			GetComponent<Score> ().Redigit (GameObject.Find ("Second").GetComponent<NameAndScore> ().score);

		} else {
			GetComponent<Score> ().Redigit (GameObject.Find ("Third").GetComponent<NameAndScore> ().score);

		}
	}
}
