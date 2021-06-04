using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {
	public int x = 1;
	public int y = 0;
	public GameObject center;
	public GameObject blockManager;
	public GameObject lines;

	public GameObject place;
	public GameObject help;
	public GameObject quit;
	public GameObject back;

	public Sprite pause;
	public Sprite help_sprite;
	Sprite original;
	public bool pflip = true;
	public bool hflip = true;
	bool mouseMove = false;

	Vector3 track;

	void Start () {
		original = GetComponent<SpriteRenderer> ().sprite;
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

			if (place.GetComponent<Over> ().hover == false) {
				place.GetComponent<Over> ().hover = true;
			}
		}

		if (track == Vector3.zero)
			mouseMove = false;

		if (hit.collider == null && mouseMove)
			place.GetComponent<Over> ().hover = false;
		
		//Whenever "Pause" is active, most components are deactivated.
		if (Input.GetButtonDown ("Pause")) {
			Check ();
		}

		if(Input.GetButtonDown("Fire1") && hit.collider != null) {
			if (hit.collider.name == "Pause") {
				Check ();
			}
			if (hit.collider.name == "Help") {
				if (hflip) {
					GetComponent<SpriteRenderer> ().sprite = help_sprite;
					GetComponent<SpriteRenderer> ().sortingLayerName = "Help";
					back.GetComponent<BoxCollider2D> ().enabled = true;
					back.GetComponent<SpriteRenderer> ().enabled = true;
					hflip = false;
				}
			}
			if (hit.collider.name == "Quit" && hflip) {
				SceneManager.LoadScene ("Menu A");
				if(GameObject.Find("Menu") != null)
					Destroy(GameObject.Find("Menu"));
			}

			if (hit.collider.name == "Back") {
				GetComponent<SpriteRenderer> ().sprite = original;
				GetComponent<SpriteRenderer> ().sortingLayerName = "Background";
				back.GetComponent<BoxCollider2D> ().enabled = false;
				back.GetComponent<SpriteRenderer> ().enabled = false;
				hflip = true;
			}
		}
	}

	void Show(GameObject box, bool yes){
		if (yes) {
			box.GetComponent<BoxCollider2D> ().enabled = true;
			box.GetComponent<SpriteRenderer> ().enabled = true;
		} else {
			box.GetComponent<BoxCollider2D> ().enabled = false;
			box.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	//The pause function. Halts all gameplay and changes the screen to a pause menu.
	void Check(){
		if (pflip) {
			center.GetComponent<SpriteRenderer> ().sprite = pause;
			center.GetComponent<SpriteRenderer> ().sortingLayerName = "Pause";
			blockManager.GetComponent<Fall> ().enabled = false;
			blockManager.GetComponent<nextTetronimo> ().enabled = false;
			blockManager.GetComponent<SpriteRenderer> ().enabled = false;
			Show (help, true);
			Show (quit, true);
			lines.GetComponent<Score> ().enabled = false;
			lines.GetComponent<lineCheck> ().enabled = false;
			lines.GetComponent<Lines> ().enabled = false;
			pflip = false;
		} else {
			center.GetComponent<SpriteRenderer> ().sprite = null;
			center.GetComponent<SpriteRenderer> ().sortingLayerName = "Quit";
			blockManager.GetComponent<Fall> ().enabled = true;
			blockManager.GetComponent<nextTetronimo> ().enabled = true;
			blockManager.GetComponent<SpriteRenderer> ().enabled = true;
			Show (help, false);
			Show (quit, false);
			lines.GetComponent<Score> ().enabled = true;
			lines.GetComponent<lineCheck> ().enabled = true;
			lines.GetComponent<Lines> ().enabled = true;
			pflip = true;
		}
	}
}
