using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class End : MonoBehaviour {

	// Activates after game-over and highscore calculation.
	void Update () {
		if (Input.anyKey) {

			//Returns the screen to the appropriate menu and destroys itself to avoid clutter.
			if(GetComponent<SpriteRenderer>().sprite.name == "Menu_A")
				SceneManager.LoadScene ("Menu A", LoadSceneMode.Single);
			else
				SceneManager.LoadScene ("Menu B", LoadSceneMode.Single);

			Destroy (this.gameObject);
		}
	}
}
