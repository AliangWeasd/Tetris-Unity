using UnityEngine;
using System.Collections;

public class selfDestruct : MonoBehaviour {
	GameObject[] another;
	// Use this for initialization
	void Awake () {
		another = GameObject.FindGameObjectsWithTag ("Stay");
		if (another.Length >= 2) {
			Destroy (gameObject);
		}
	}
}
