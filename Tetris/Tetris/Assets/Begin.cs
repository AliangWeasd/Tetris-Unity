using UnityEngine;
using UnityEngine.SceneManagement;

public class Begin : MonoBehaviour {
	public GameObject scores;
	// Use this for initialization

	void Awake () {
		DontDestroyOnLoad (scores);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			SceneManager.LoadScene ("Main Menu", LoadSceneMode.Single);
		}
	}
}
