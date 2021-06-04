using UnityEngine;
using System.Collections;

public class lineCheck : MonoBehaviour {

	public int [] add10 = new int [20];
	public GameObject blockManager;
	public GameObject manager;
	public Sprite empty;
	public Sprite emptyLine;
	public Sprite yellowLine;
	public Sprite brickLine;
	public Sprite gameOver;

	GameObject first;
	GameObject second;
	GameObject third;

	int n = 1;
	int y = 1;
	int height = 0;
	int count = 0;
	public bool passed = false;

	//Immediately finds the score modifier and highscore objects.
	void Start(){
		n = GameObject.Find ("Menu").GetComponent<MenuMoveA> ().number + 1;

		first = GameObject.Find ("First");
		second = GameObject.Find ("Second");
		third = GameObject.Find ("Third");
	}

	//First checks if a line has been made.
	//If so, then the line is cleared and the blocks above are moved down.
	public IEnumerator Check(){
		//By ending at the height, the check only looks over lines that has blocks in the first place.
		while (y < height + 1) {
			if (add10 [y] >= 10) {
				StartCoroutine (Yellow());
				count++;
				passed = true;
			}
			y++;
		}
		y = 1;

		//If "passed" is true, then a line was found.
		if(passed){
			//Disable turning for the meanwhile.
			blockManager.GetComponent<Fall>().turnable = false;
			//A 0.9 second period where pausing does not disrupt the "line-clearing" animation.
			yield return new WaitForSeconds (0.9f);

			while (manager.GetComponent<Buttons> ().pflip == false) {
				yield return null;
			}
			yield return new WaitForSeconds (0.3f);

			while (y < height + 1) {
				//If the line had 10 blocks...
				y++;
				if (add10 [y - 1] >= 10) {
					//...then clear the line.
					for (int h = 1; h < 11; h++) {
						blockManager.GetComponent<Fall> ().Clear (h, y - 1);
					}
					y--;
					//Then drop every other block above the cleared line.
					for (int b = y; b < height + 1; b++) {
						add10 [b] = add10 [b + 1];
						for (int a = 1; a < 11; a++) {
							if (GameObject.Find (a + "," + b + "(Clone)").tag == "Filled") {
								GameObject.Find (a + "," + (b) + "(Clone)").GetComponent<Occupied> ().DropMove (a, b - 1);
								blockManager.GetComponent<Fall> ().Clear (a, b);
							} else
								blockManager.GetComponent<Fall> ().Clear (a, b);
						}
					}
					height--;
				}
			}

			blockManager.GetComponent<Fall>().turnable = true;
		}

		//Measures how much score should be added.
		if (count != 0) {
			int tempScore = 0;
			if (count == 4)
				tempScore = 1200 * n;
			else if (count == 3)
				tempScore = 300 * n;
			else if (count == 2)
				tempScore = 100 * n;
			else if (count == 1)
				tempScore = 40 * n;
			GetComponent<Score> ().Redigit(tempScore);
		}

		GetComponent<Lines> ().Add (count);
		count = 0;

		passed = false;
		y = 1;

		if(!blockManager.GetComponent<Fall>().end)
			blockManager.GetComponent<Fall> ().resetable = true;
		
		if (GetComponent<lineCheck> ().enabled == true) {
			blockManager.GetComponent<Fall> ().Reset();
			yield return null;
		}
	}

	//Plays the animation for losing the game.
	//The first block in the array becomes a brick line.
	//Also deletes the game blocks and BlockManager.
	public IEnumerator End(){
		GameObject.Find("Manager").GetComponent<Buttons> ().enabled = false;

		for (int p = 0; p < 20; p++) {
			GameObject.Find("1," + p + "(Clone)").GetComponent<SpriteRenderer>().sprite = brickLine;
			yield return new WaitForSeconds (0.01f);
		}

		for (int h = 0; h < 20; h++)
			for (int k = 0; k < 12; k++)
				if (k != 1)
					Destroy (GameObject.Find (k + "," + h + "(Clone)"));

		GameObject.Find ("Center").GetComponent<SpriteRenderer> ().sprite = gameOver;

		//Code used for changing the high score ranks.
		//Searchs for new 1st scores, then 2nd, then 3rd.
		int total = GetComponent<Score> ().total;
		if (total > first.GetComponent<NameAndScore> ().score) {
			third.GetComponent<NameAndScore> ().score = second.GetComponent<NameAndScore> ().score;
			second.GetComponent<NameAndScore> ().score = first.GetComponent<NameAndScore> ().score;
			first.GetComponent<NameAndScore> ().score = total;
		} else if (total > second.GetComponent<NameAndScore> ().score) {
			third.GetComponent<NameAndScore> ().score = second.GetComponent<NameAndScore> ().score;
			second.GetComponent<NameAndScore> ().score = total;
		} else if (total > third.GetComponent<NameAndScore> ().score) {
			third.GetComponent<NameAndScore> ().score = total;
		}

		yield return new WaitForSeconds (1f);

		for (int l = 0; l < 20; l++){
			Destroy (GameObject.Find ("1," + l + "(Clone)"));
			yield return new WaitForSeconds (0.01f);
		}

		//The object that started the coroutine must still remain. Otherwise, the coroutine ends.
		Destroy (blockManager);

		if(GameObject.Find ("Menu") != null)
			GameObject.Find ("Menu").GetComponent<End> ().enabled = true;

		yield return null;
	}

	//Plays the animation for completing a line.
	//The first block in the line switches from being a yellow line to a clear line.
	IEnumerator Yellow(){
		string blockName = "1," + y + "(Clone)";

		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = yellowLine;
		yield return new WaitForSeconds (0.18f);
		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = emptyLine;
		yield return new WaitForSeconds (0.18f);
		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = yellowLine;
		yield return new WaitForSeconds (0.18f);
		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = emptyLine;
		yield return new WaitForSeconds (0.18f);
		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = yellowLine;
		yield return new WaitForSeconds (0.18f);
		GameObject.Find(blockName).GetComponent<SpriteRenderer>().sprite = emptyLine;

		yield return null;
	}

	//Updates the max height of the blocks.
	public void ReCheck(int y1, int y2, int y3, int y4){
		add10 [y1]++;
		add10 [y2]++;
		add10 [y3]++;
		add10 [y4]++;

		if (y1 > height)
			height = y1;
		if (y2 > height)
			height = y2;
		if (y3 > height)
			height = y3;
		if (y4 > height)
			height = y4;
	}
}