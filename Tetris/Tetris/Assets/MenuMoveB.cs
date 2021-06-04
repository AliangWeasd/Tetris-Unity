using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuMoveB : MonoBehaviour {

	public int x = 1;
	public int y = 0;

	GameObject place;

	Vector3 track;
	Vector3 other;

	bool mouseMove = false;

	void Start () {
		place = GameObject.Find(y + "," + x);
	}

	void Update () {
		Vector3 mPos3D = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 pos = new Vector2(mPos3D.x,mPos3D.y);

		Debug.DrawRay (pos, Vector3.back*5);

		RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);

		track = new Vector3 (Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

		if (hit.collider != null && track != Vector3.zero) {
			place.GetComponent<Over> ().hover = false;
			mouseMove = true;

			place = GameObject.Find (hit.collider.name);
			y = int.Parse (hit.collider.name.Substring(0,1));
			if (y != 0)
				x = int.Parse (hit.collider.name.Substring (2));

			if (place.GetComponent<Over> ().hover == false) {
				place.GetComponent<Over> ().hover = true;
			}
		}

		if (track == Vector3.zero)
			mouseMove = false;

		if (hit.collider == null && mouseMove)
			place.GetComponent<Over> ().hover = false;
		
		if(Input.GetButtonDown("Horizontal")){
			if(y != 0)
				place.GetComponent<Over> ().hover = false;

			if (Input.GetAxisRaw ("Horizontal") > 0 && x < 7)
				x++;
			else if(Input.GetAxisRaw ("Horizontal") < 0 && x > 0)
				x--;

			if (y != 0) {
				place = GameObject.Find (y + "," + x);
				place.GetComponent<Over> ().hover = true;
			}
		}

		if(Input.GetButtonDown("Vertical")){
			place.GetComponent<Over> ().hover = false;

			if (Input.GetAxisRaw ("Vertical") > 0 && y > 0)
				y--;
			else if(Input.GetAxisRaw ("Vertical") < 0 && y < 2)
				y++;

			if (y == 0)
				place = GameObject.Find ("0");
			else
				place = GameObject.Find(y + "," + x);

			place.GetComponent<Over> ().hover = true;
		}

		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit") || ((Input.GetButtonDown ("Fire1") && hit.collider != null))) {
			if (place.name == "0") {
				SceneManager.LoadScene ("Main Menu", LoadSceneMode.Single);
			}
		}
	}
}
