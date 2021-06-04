using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuMoveA : MonoBehaviour {
	
	public int x = 1;
	public int y = 0;
	public int number = 0;

	public GameObject place;
	public Sprite save;

	Vector3 track;

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
			if (place.tag != "Menu") {
				save = place.GetComponent<Over> ().states[1];
				number = int.Parse (place.tag);
			}
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
			
			if (Input.GetAxisRaw ("Horizontal") > 0 && x < 5)
				x++;
			else if (Input.GetAxisRaw ("Horizontal") < 0 && x > 1)
				x--;

			if (y != 0) {
				place = GameObject.Find (y + "," + x);
				place.GetComponent<Over> ().hover = true;
			}

			if (place.tag != "Menu") {
				number = int.Parse (place.tag);
				save = place.GetComponent<Over> ().states[1];
			}
		}

		if(Input.GetButtonDown("Vertical")){
			place.GetComponent<Over> ().hover = false;

			if (Input.GetAxisRaw ("Vertical") > 0 && y > 0)
				y--;
			else if (Input.GetAxisRaw ("Vertical") < 0 && y < 2)
				y++;

			if (y == 0)
				place = GameObject.Find ("0");
			else
				place = GameObject.Find(y + "," + x);
			
			place.GetComponent<Over> ().hover = true;

			if (place.tag != "Menu") {
				number = int.Parse (place.tag);
				save = place.GetComponent<Over> ().states[1];
			}
		}

		if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit") || ((Input.GetButtonDown ("Fire1") && hit.collider != null))) {
			if (place.name == "0") {
				SceneManager.LoadScene ("Main Menu", LoadSceneMode.Single);
			}
			else {
				DontDestroyOnLoad (this);
				GetComponent<MenuMoveA> ().enabled = false;
				SceneManager.LoadScene ("Tetris", LoadSceneMode.Single);
			}
		}
	}
}
