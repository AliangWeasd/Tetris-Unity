using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuMove : MonoBehaviour {
	public int x = 1;
	public int y = 1;

	GameObject place;
	GameObject gameLast;
	GameObject musicLast;

	Vector3 track;

	bool mouseMove = false;

	void Start () {
		place = GameObject.Find(y + "," + x);
		gameLast = GameObject.Find ("1,1");
		musicLast = GameObject.Find ("2,1");
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

			if(place != GameObject.Find(hit.collider.name) && place.GetComponent<Over>().gameLocked)
				gameLast = place;
			if(place != GameObject.Find(hit.collider.name) && place.GetComponent<Over>().musicLocked)
				musicLast = place;
			
			place = GameObject.Find (hit.collider.name);
			y = int.Parse (hit.collider.name.Substring(0,1));
			if (y != 4)
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
			if (y != 4) {
				place.GetComponent<Over> ().hover = false;
			}

			if (Input.GetAxisRaw ("Horizontal") > 0 && x == 1)
				x++;
			else if(Input.GetAxisRaw ("Horizontal") < 0 && x == 2)
				x--;

			if (place != GameObject.Find (y + "," + x)) {
				if (place.GetComponent<Over> ().gameLocked)
					gameLast = place;
				if (place.GetComponent<Over> ().musicLocked)
					musicLast = place;
			}
			
			if (y != 4) {
				place = GameObject.Find (y + "," + x);
				place.GetComponent<Over> ().hover = true;
			}
		}

		if(Input.GetButtonDown("Vertical")){
			place.GetComponent<Over> ().hover = false;

			if (Input.GetAxisRaw ("Vertical") > 0 && y > 0)
				y--;
			else if(Input.GetAxisRaw ("Vertical") < 0 && y < 4)
				y++;

			if (place != GameObject.Find (y + "," + x)) {
				if (place.GetComponent<Over> ().gameLocked)
					gameLast = place;
				if (place.GetComponent<Over> ().musicLocked)
					musicLast = place;
			}
			
			if(y == 4)
				place = GameObject.Find(y + "");
			else
				place = GameObject.Find(y + "," + x);
			
			place.GetComponent<Over> ().hover = true;
		}

		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit") || ((Input.GetButtonDown ("Fire1") && hit.collider != null))) {
			if (place.name == "0,2") {
				SceneManager.LoadScene ("Title Screen", LoadSceneMode.Single);
			}
			if (place.name == "1,1" || place.name == "1,2"){
				gameLast.GetComponent<Over> ().gameLocked = false;
				place.GetComponent<Over> ().gameLocked = true;
			}
			if (place.name == "2,1" || place.name == "2,2" || place.name == "3,1" || place.name == "3,2") {
				musicLast.GetComponent<Over> ().musicLocked = false;
				place.GetComponent<Over> ().musicLocked = true;
			}
			if (place.name == "4") {
				if(gameLast.name == "1,1")
					SceneManager.LoadScene ("Menu A", LoadSceneMode.Single);
				if(gameLast.name == "1,2")
					SceneManager.LoadScene ("Menu B", LoadSceneMode.Single);
			}
		}
	}
}
