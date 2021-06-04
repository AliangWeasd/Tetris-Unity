using UnityEngine;
using System.Collections;

public class mouseTrack : MonoBehaviour {

	// Update is called once per frame
	public Vector3 MouseUpdate (Vector3 mPos3D, Vector3 track, RaycastHit2D hit) {
		mPos3D = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 pos = new Vector2(mPos3D.x,mPos3D.y);

		Debug.DrawRay (pos, Vector3.back*5);

		hit = Physics2D.Raycast (pos, Vector2.zero);

		track = new Vector3 (Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

		return track;
	}

	public void MouseStop(){

	}
}
